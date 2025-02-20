
using Onze_11_Visualstudio.Models;
using Onze_11_Visualstudio.ORM;
using System.Linq.Expressions;

namespace SiteAgendamento.Repositorio
{
    public class RelatorioRepositorio
    {

        private BdOnzeContext _context;
        public RelatorioRepositorio(BdOnzeContext context)
        {
            _context = context;
        }

        public List<ViewAgendamento> GetAgendamentos(
    string campo1, string campo2, string campo3,
    string valor1, string valor2, string valor3)
        {
            var query = _context.ViewAgendamentos.AsQueryable();

            // Adiciona filtro para o primeiro campo e valor
            if (!string.IsNullOrEmpty(campo1) && !string.IsNullOrEmpty(valor1))
            {
                query = query.Where(FiltroDinamico(campo1, valor1));
            }

            // Adiciona filtro para o segundo campo e valor
            if (!string.IsNullOrEmpty(campo2) && !string.IsNullOrEmpty(valor2))
            {
                query = query.Where(FiltroDinamico(campo2, valor2));
            }

            // Adiciona filtro para o terceiro campo e valor
            if (!string.IsNullOrEmpty(campo3) && !string.IsNullOrEmpty(valor3))
            {
                query = query.Where(FiltroDinamico(campo3, valor3));
            }

            // Limita a 1000 registros (como na consulta original)
            var agendamentos = query
                .OrderBy(a => a.DtHoraAgendamento)
                .Take(1000)
                .ToList();

            return agendamentos;
        }

        // Método auxiliar para criar a expressão de filtro dinamicamente
        private static Expression<Func<ViewAgendamento, bool>> FiltroDinamico(string campo, string valor)
        {
            switch (campo.ToLower())
            {
                case "tiposervico":
                    return a => a.TipoServico.Contains(valor, StringComparison.OrdinalIgnoreCase);
                case "nome":
                    return a => a.Nome.Contains(valor, StringComparison.OrdinalIgnoreCase);
                case "email":
                    return a => a.Email.Contains(valor, StringComparison.OrdinalIgnoreCase);
                case "telefone":
                    return a => a.Telefone.Contains(valor, StringComparison.OrdinalIgnoreCase);
                case "dtagendamento":
                    if (DateTime.TryParse(valor, out DateTime dateValue))
                    {
                        return a => a.DtHoraAgendamento.Date == dateValue.Date;
                    }
                    return a => false; // Filtro inválido
                default:
                    return a => true; // Sem filtro
            }
        }


    }
}
