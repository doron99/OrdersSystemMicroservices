using Client.API.Models;

namespace Client.API.Services
{
    public static class FakeCustomerService
    {
        public static List<Customer> GetCustomers()
        {
            List<Customer> cl = new List<Customer>();
            cl.Add(new Customer { CustomerId = Guid.Parse("612e53ca-f640-484d-bbb1-5bd98c3f54ea"), CustomerName = "Skyline Builders" });
            cl.Add(new Customer { CustomerId = Guid.Parse("79e07974-b7ad-4b60-92c3-e77d074f1046"), CustomerName = "Everest Construction Group" });
            cl.Add(new Customer { CustomerId = Guid.Parse("0abec15c-3ff1-476e-a08b-cf82c0f8c124"), CustomerName = "UrbanEdge Contractors" });
            cl.Add(new Customer { CustomerId = Guid.Parse("857101ff-8967-4c38-832d-d032f18014c6"), CustomerName = "BlueRock Developments" });
            cl.Add(new Customer { CustomerId = Guid.Parse("7dc1e971-93a4-46c3-98c1-2249b4486297"), CustomerName = "PrimeStone Construction" });
            cl.Add(new Customer { CustomerId = Guid.Parse("7ef94ed8-d7d2-45fc-8d3e-662c201094bf"), CustomerName = "GoldenGate Builders" });
            cl.Add(new Customer { CustomerId = Guid.Parse("a78a71df-e0e5-4879-b0a0-0f53921674ae"), CustomerName = "SolidCore Engineering" });
            cl.Add(new Customer { CustomerId = Guid.Parse("700759b2-e83f-4c87-94a1-dcfba14203c7"), CustomerName = "Titan Construction Co." });
            cl.Add(new Customer { CustomerId = Guid.Parse("f8039afe-6b23-41d4-91e7-3efe295b2154"), CustomerName = "NextGen Infrastructure" });
            cl.Add(new Customer { CustomerId = Guid.Parse("9e7cfe74-f723-4faf-bd14-0c29e512fbe6"), CustomerName = "Summit Builders Ltd." });
            cl.Add(new Customer { CustomerId = Guid.Parse("bbfbccba-3871-4931-a1f6-18d81a3585a0"), CustomerName = "Ironclad Construction" });
            cl.Add(new Customer { CustomerId = Guid.Parse("87c23088-40ee-461b-bbce-6530161678ce"), CustomerName = "MetroBuild Group" });
            cl.Add(new Customer { CustomerId = Guid.Parse("96a95dfe-5aba-4704-9b0b-db6194949c2a"), CustomerName = "CrestPoint Constructors" });
            cl.Add(new Customer { CustomerId = Guid.Parse("96f4cca0-8409-4be2-b71b-e16eb1f4bc4a"), CustomerName = "Oakridge Engineering" });
            cl.Add(new Customer { CustomerId = Guid.Parse("dd8294c5-687a-4a04-a2c7-497471808b07"), CustomerName = "Vertex Construction Solutions" });
            cl.Add(new Customer { CustomerId = Guid.Parse("a5391aee-d68c-4d0f-91f4-849cf21d0e4e"), CustomerName = "Pioneer Builders Inc." });
            cl.Add(new Customer { CustomerId = Guid.Parse("06a8868d-d07d-421b-9dc6-729bfa297c0d"), CustomerName = "Bridgeway Developments" });
            cl.Add(new Customer { CustomerId = Guid.Parse("9fe0058a-5d27-414a-9994-4a48df9a1f3d"), CustomerName = "Apex Construction & Design" });
            cl.Add(new Customer { CustomerId = Guid.Parse("731c24bf-36d5-40e3-a5c1-225b3046034a"), CustomerName = "NorthStar Contractors" });
            cl.Add(new Customer { CustomerId = Guid.Parse("f2e4db72-8a6a-4c80-8bf7-53ac6f780559"), CustomerName = "Silverline Builders" });
            return cl;
        }
    }
}
