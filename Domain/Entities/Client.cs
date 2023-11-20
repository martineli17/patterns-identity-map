namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        protected Client() { }

        public Client(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
