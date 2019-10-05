using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository.Data;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
         _context = context;
         _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        //GERAIS
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        //EVENTO
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedeSociais);

            if(includePalestrantes) {
                query  = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrantes);
            }

            query = query.OrderByDescending(o => o.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventoAsyncById(int eventoId, bool includePalestrantes = false)
        {
             IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedeSociais);

            if(includePalestrantes) {
                query  = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrantes);
            }

            query = query.OrderByDescending(o => o.DataEvento).Where(w => w.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
             IQueryable<Evento> query = _context.Eventos.Include(c => c.Lotes).Include(c => c.RedeSociais);

            if(includePalestrantes) {
                query  = query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrantes);
            }

            query = query.OrderByDescending(o => o.DataEvento).Where(w => w.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }


        //PALESTRANTE
        public async Task<Palestrante[]> GetAllPalestranteAsyncByName(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(r => r.RedeSociais);

            if(includeEventos) {
                query  = query.Include(pe => pe.PalestranteEventos).ThenInclude(e => e.Evento);
            }

            query = query.Where(w => w.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(r => r.RedeSociais);

            if(includeEventos) {
                query  = query.Include(pe => pe.PalestranteEventos).ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(o => o.Nome).Where(w => w.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
     }
}