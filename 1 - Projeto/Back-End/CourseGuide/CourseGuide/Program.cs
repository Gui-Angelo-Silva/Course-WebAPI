namespace CourseGuide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Inicia o aplicativo e executa o host.
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) // Cria um host padr�o com configura��es comuns.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Define a classe Startup como ponto de entrada para configura��o do aplicativo.
                });
    }
}
