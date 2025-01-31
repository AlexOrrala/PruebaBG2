using liquidacion_ajoo.Models;
using Microsoft.EntityFrameworkCore;

namespace liquidacion_ajoo.Services
{
    public class ClientesServices
    {
        private readonly ApplicationDbcontext _context;

        public ClientesServices(ApplicationDbcontext context)
        {
            _context = context;
        }

        internal async Task<Clientes_ajoo> validarId(int id)
        {
            Clientes_ajoo cliente  = await _context.clientes_ajoos.FirstOrDefaultAsync(c => c.Id == id);


            if (cliente == null)
            {
                throw new ArgumentException("El Id del cliente debe existir");
            }
            Console.WriteLine(cliente.ToString());

            return cliente;
        }

        public async Task<Clientes_ajoo> AddCliente(Clientes_ajoo cliente)
        {
            try
            {
                await _context.clientes_ajoos.AddAsync(cliente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return cliente;
        }


    }
}