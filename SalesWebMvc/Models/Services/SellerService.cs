using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models.Services.Exceptions;

namespace SalesWebMvc.Models.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Seller.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);

                _context.Seller.Remove(seller);

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            if(!await _context.Seller.AnyAsync(x => x.Id == seller.Id)) 
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Seller.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) 
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
            
        }
    }
}
