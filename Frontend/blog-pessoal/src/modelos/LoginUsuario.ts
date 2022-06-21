interface LoginUsuario {
    id: number;
    usuario: string;
    senha: string;
    token?: string| null
}

export default LoginUsuario;