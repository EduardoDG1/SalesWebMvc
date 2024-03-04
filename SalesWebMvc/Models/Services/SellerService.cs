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

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller seller)
        {
            _context.Seller.Add(seller);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(s => s.Id == id);
        }

        public void Remove(int id)
        {
            var seller = _context.Seller.Find(id);

            _context.Seller.Remove(seller);

            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            if(!_context.Seller.Any(x => x.Id == seller.Id)) 
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Seller.Update(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) 
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
            
        }
    }
}
