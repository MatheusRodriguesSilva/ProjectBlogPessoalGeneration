using System.Linq;
using System.Threading.Tasks;
using BlogPessoal.src.contextos;
using BlogPessoal.src.modelos;
using BlogPessoal.src.repositorios;
using BlogPessoal.src.repositorios.implementacoes;
using BlogPessoal.src.utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogPessoalTeste.Testes.repositorios
{
    [TestClass]
    public class PostagemRepositorioTeste
    {
        private BlogPessoalContexto _contexto;
        private IUsuario _repositorioU;
        private ITema _repositorioT;
        private IPostagem _repositorioP;

        [TestMethod]
        public async Task CriaTresPostagemNoSistemaRetornaTres()
        {
            // Definindo o contexto
           var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal21")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 2 usuarios
            await _repositorioU.NovoUsuarioAsync(new Usuario
            {
                Nome = "Matheus Rodrigues",
                Email = "matheus@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            await _repositorioU.NovoUsuarioAsync(new Usuario
            {
                Nome = "Pamela Boaz",
                Email = "pamela@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            // AND - E que registro 2 temas
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "C#" });
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "Java" });

            // WHEN - Quando registro 3 postagens
            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é muito legal",
                Descricao = "dá para programar de diversas formas",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("gustavo@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(1)
            });

            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é muito bom",
                Descricao = "muitas formas de programar",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("catarina@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(1)
            });

            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é da hora",
                Descricao = "C# é uma ótima linguagem",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("gustavo@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(2)
            });
            
            // WHEN - Quando eu busco todas as postagens
            var postagens = await _repositorioP.PegarTodasPostagensAsync();

            // THEN - Eu tenho 3 postagens
            Assert.AreEqual(3, postagens.Count);
        }

        [TestMethod]
        public async Task AtualizarPostagemRetornaPostagemAtualizada()
        {
            // Definindo o contexto
            var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal22")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 1 usuarios
            await _repositorioU.NovoUsuarioAsync(new Usuario
            {
                Nome = "Matheus Rodrigues",
                Email = "matheus@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            // AND - E que registro 1 tema
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "COBOL" });
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "C#" });

            // AND - E que registro 1 postagem
            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "Programar é da hora",
                Descricao = "adquiro conhecimento",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("gustavo@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(1)
            });

            // WHEN - Quando atualizo postagem de id 1
            await _repositorioP.AtualizarPostagemAsync(new Postagem
            {
                Id = 1,
                Titulo = "CSS é fácil",
                Descricao = "CSS é muito bom",
                Foto = "URLDAFOTOATUALIZADA",
                Tema = await _repositorioT.PegarTemaPeloIdAsync(2)
            });

            var postagem = await _repositorioP.PegarPostagemPeloIdAsync(1);

            // THEN - Eu tenho a postagem atualizada
            Assert.AreEqual("C# é dahora", postagem.Titulo);
            Assert.AreEqual("C# é uma ótima linguagem", postagem.Descricao);
            Assert.AreEqual("URLDAFOTOATUALIZADA", postagem.Foto);
            Assert.AreEqual("C#", postagem.Tema.Descricao);
        }

        [TestMethod]
        public async Task PegarPostagensPorPesquisaRetodarCustomizada()
        {
            // Definindo o contexto
            var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal23")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 2 usuarios
            await _repositorioU.NovoUsuarioAsync(new Usuario
            {
                Nome = "Matheus Rodrigues",
                Email = "matheus@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            await _repositorioU.NovoUsuarioAsync(new Usuario
            {
                Nome = "Pamela Boaz",
                Email = "catarina@email.com",
                Senha = "134652",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            // AND - E que registro 2 temas
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "C#" });
            await _repositorioT.NovoTemaAsync(new Tema { Descricao = "Java" });

            // WHEN - Quando registro 3 postagens
            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é muito massa",
                Descricao = "É uma linguagem muito utilizada no mundo",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("gustavo@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(1)
            });

            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é muito bom",
                Descricao = "muitas formas de programar",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("catarina@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(1)
            });

            await _repositorioP.NovaPostagemAsync(new Postagem
            {
                Titulo = "C# é da hora",
                Descricao = "C# é uma ótima linguagem",
                Foto = "URLDAFOTO",
                Criador = await _repositorioU.PegarUsuarioPeloEmailAsync("catarina@email.com"),
                Tema = await _repositorioT.PegarTemaPeloIdAsync(2)
            });

            var postagensTeste1 = await _repositorioP.PegarPostagensPorPesquisaAsync("massa", null, null);
            var postagensTeste2 = await _repositorioP.PegarPostagensPorPesquisaAsync(null, "C#", null);
            var postagensTeste3 = await _repositorioP.PegarPostagensPorPesquisaAsync(null, null, "gustavo@email.com");

            // WHEN - Quando eu busco as postagen
            // THEN - Eu tenho as postagens que correspondem aos criterios
            Assert.AreEqual(2, postagensTeste1.Count);
            Assert.AreEqual(2, postagensTeste2.Count);
            Assert.AreEqual(1, postagensTeste3.Count);
        }
    }
}