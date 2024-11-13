using Microsoft.Extensions.Configuration;
using YourGT.DataAccess.EFCore;
using YourGT.DataAccess.EFCore.Repositories;
using YourGT.Shared.Entities;

namespace YourGT.ConsoleClient;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        var context = new YourGTDbContext(config);

        Test.Education(context);

        //var education = new Education();
        //education.Name = "TestEducation1";
        //education.Description = "TestDescription1";
        //education.Completed = false;
        //education.Semesters = 4;

        //var educationRepository = new EducationRepository(context);

        //educationRepository.DeleteById(1);

        //var education = educationRepository.GetById(1);

        //education.Name = "TestEducation01";
        //education.Description = "TestDescription01";
        //education.Completed = true;
        //education.Semesters = 6;

        //educationRepository.Update(1, education);

        //educationRepository.Add(education);

        //var educations = educationRepository.GetAll();

        //foreach (var e in educations)
        //{
        //    Console.WriteLine($"Education: {e.Id}");
        //    Console.WriteLine($"Name: {e.Name}");
        //    Console.WriteLine($"Description: {e.Description}");
        //    Console.WriteLine($"Semesters: {e.Semesters}");
        //    Console.WriteLine($"Completed: {e.Completed}");

        //}
        Console.WriteLine();
        Console.WriteLine("Hello, World!");

        //var connectionString = config["ConnectionStrings:Default"];

        //Console.WriteLine(connectionStrings);
    }
}
