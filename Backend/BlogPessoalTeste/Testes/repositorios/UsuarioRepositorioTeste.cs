using System.Linq;
using System.Threading.Tasks;
using BlogPessoal.src.contextos;
using BlogPessoal.src.modelos;
using BlogPessoal.src.repositorios;
using BlogPessoal.src.repositorios.implementacoes;
using BlogPessoal.src.utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogPessoalTest.Testes.repositorios
{
    [TestClass]
    public class UsuarioRepositorioTeste
    {
        private BlogPessoalContexto _contexto;
        private IUsuario _repositorio;

        [TestMethod]
        public async Task CriarQuatroUsuariosNoBancoRetornaQuatroUsuarios()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal1")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro 4 usuarios no banco
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Matheus Rodrigues",
                Email = "matheus@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Pamela Boaz",
                Email = "pamela@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Isa Boaz",
                Email = "Isa@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
 
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Karol Boaz",
                Email = "Karol@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
			//WHEN - Quando pesquiso lista total            
            //THEN - Ent??o recebo 4 usuarios
            Assert.AreEqual(4, _contexto.Usuarios.Count());
        }
        
        [TestMethod]
        public async Task PegarUsuarioPeloEmailRetornaNaoNulo()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal2")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Lucas Boaz",
                Email = "lucas@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            //WHEN - Quando pesquiso pelo email deste usuario
            var usuario = await _repositorio.PegarUsuarioPeloEmailAsync("lucas@email.com");
			
            //THEN - Ent??o obtenho um usuario
            Assert.IsNotNull(usuario);
        }

        [TestMethod]
        public async Task PegarUsuarioPeloIdRetornaNaoNuloENomeDoUsuario()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal3")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Guilherme Boaz",
                Email = "guilherme@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
			//WHEN - Quando pesquiso pelo id 1
            var usuario = await _repositorio.PegarUsuarioPeloIdAsync(1);

            //THEN - Ent??o, deve me retornar um elemento n??o nulo
            Assert.IsNotNull(usuario);
            //THEN - Ent??o, o elemento deve ser Neusa Boaz
            Assert.AreEqual("Guilherme Boaz", usuario.Nome);
        }

        [TestMethod]
        public async Task AtualizarUsuarioRetornaUsuarioAtualizado()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal4")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(new Usuario
            {
                Nome = "Henrique Boaz",
                Email = "henrique@email.com",
                Senha = "123456",
                Foto = "URLFOTO",
                Tipo = TipoUsuario.NORMAL
            });
            
            //WHEN - Quando atualizamos o usuario
            await _repositorio.AtualizarUsuarioAsync(new Usuario
            {
                Id = 1,
                Nome = "Eduardo Boaz",
                Senha = "123456",
                Foto = "URLFOTONOVA",
                Tipo = TipoUsuario.NORMAL
            });
            
            //THEN - Ent??o, quando validamos pesquisa deve retornar nome Estef??nia Moura
            var antigo = await _repositorio.PegarUsuarioPeloEmailAsync("estefania@email.com");

            Assert.AreEqual(
                "Eduardo Boaz",
                _contexto.Usuarios.FirstOrDefault(u => u.Id == antigo.Id).Nome
            );
            
            //THEN - Ent??o, quando validamos pesquisa deve retornar senha 123456
            Assert.AreEqual(
                "123456",
                _contexto.Usuarios.FirstOrDefault(u => u.Id == antigo.Id).Senha
            );
        }
    }
}