using backend2.Interfaces;
using backend2.Modelos;
using Microsoft.EntityFrameworkCore;


namespace backend2.Servicios {
    public class SesionService : ISesionService {
        private readonly IUnitOfWork _repositorioEspecifico;
        private readonly IConfiguration _configuration;
        public SesionService(IUnitOfWork repositorioEspecifico, IConfiguration configuration) {
            _repositorioEspecifico = repositorioEspecifico;
            _configuration = configuration;
        }

        public async Task<Usuario?> CheckUser(string email, string pass) {
            string encrypt = Criptografia.GeneratePassword.HashPassword(pass);
            Usuario userLogin = await _repositorioEspecifico.UsuariosRepositorio.GetAll()
                .FirstOrDefaultAsync(u => u.Email == email && u.Contraseña == encrypt);

            return userLogin;
        }

    }
}


