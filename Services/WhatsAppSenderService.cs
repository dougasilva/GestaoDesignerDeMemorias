namespace GestaoDesignerDeMemorias.Services
{
    public class WhatsAppSenderService
    {
        // Por enquanto vamos só simular o envio
        // Depois conectamos com Evolution API, Wati, etc.

        public async Task EnviarMensagemAsync(string whatsapp, string mensagem)
        {
            Console.WriteLine($"📤 [SIMULADO] Enviando para {whatsapp}:");
            Console.WriteLine($"   {mensagem}");
            Console.WriteLine($"   Status: Enviado com sucesso ✅\n");

            // TODO: Implementar integração real com provedor aqui
            await Task.CompletedTask;
        }
    }
}