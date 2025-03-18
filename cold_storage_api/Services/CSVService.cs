using cold_storage_api.Models.Dtos;
using System.Text;

namespace cold_storage_api.Services
{
    public class CSVService
    {
        public byte[] CreateCsvFile(List<Payload> payloads)
        {
            if (payloads == null || payloads.Count == 0)
            {
                throw new ArgumentException("The payloads list cannot be null or empty", nameof(payloads));
            }

            // Criação de um StringBuilder para armazenar o conteúdo do CSV
            StringBuilder csvContent = new();

            // Adiciona o cabeçalho do CSV
            csvContent.AppendLine("Key,Value");

            // Adiciona cada registro no formato CSV
            foreach (Payload payload in payloads)
            {
                csvContent.AppendLine($"{payload.Key},{payload.Value}");
            }

            // Converte o conteúdo CSV em um array de bytes
            return Encoding.UTF8.GetBytes(csvContent.ToString());
        }
    }
}
