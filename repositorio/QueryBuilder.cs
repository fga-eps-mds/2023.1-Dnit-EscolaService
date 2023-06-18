using System.Text;

namespace repositorio
{
    public class QueryBuilder
    {
        private StringBuilder queryBuilder;

        public QueryBuilder()
        {
            queryBuilder = new StringBuilder();
        }

        public QueryBuilder Select(string[] colunas)
        {
            string colunasString = string.Join(", ", colunas);
            colunasString = colunasString.TrimEnd(',', ' ');
            queryBuilder.AppendLine("SELECT");
            queryBuilder.AppendLine(colunasString);
            return this;
        }

        public QueryBuilder From(string tabela)
        {
            queryBuilder.AppendLine($"FROM {tabela}");
            return this;
        }

        public QueryBuilder Join(string tabela, string colunaEsquerda, string colunaDireita)
        {
            queryBuilder.AppendLine($"LEFT JOIN {tabela} ON {colunaEsquerda} = {colunaDireita}");
            return this;
        }

        public QueryBuilder Where(string colunaEsquerda, string colunaDireita)
        {
            queryBuilder.AppendLine($"WHERE {colunaEsquerda} = {colunaDireita}");
            return this;
        }

        public string Build()
        {
            return queryBuilder.ToString();
        }
    }
}
