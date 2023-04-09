using Alumni_Back.Models;
using Alumni_Back.Repository;

namespace Alumni_Back.Services
{
    public class FakerService:IFaker
    {
        private readonly IUserRepository _user;

        public FakerService(IUserRepository user)
        {
            _user = user;
        }

        public async Task GenerateUser()
        {
            for(int i = 1; i <= 30; i++)
            {
                string password = "123456789";
                string fakemail = $"fakemail{i}@gmail.com";
                string fakeusername = $"user{i}";
                var result = _user.Create(new DTO.UserDto
                {
                    Firstname = Faker.Name.First(),
                    Lastname = Faker.Name.Last(),
                    Gender = Faker.Boolean.Random(),
                    Birthdate = new DateTime(2003, 10, 13),
                    Address = new DTO.AddressDto
                    {
                        country = Faker.Address.Country(),
                        city = Faker.Address.City(),
                        postalcode = Faker.RandomNumber.Next(999)
                    },

                    Password = password,
                    ConfirmPassword = password,
                    Contact = Faker.Phone.Number(),
                    Email = fakemail,
                    Username = fakeusername
                });
                result.Switch(
                    async user =>
                    {
                        await _user.UpdatePicture(result.AsT0, MediaType.PDP, $"{Faker.RandomNumber.Next(1, 7)}.png");
                    },
                    error =>
                    {
                    });
            }
        }
    }
}
