using IAF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IAF.Services
{
    public class IAFService
    {
        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }
        public async Task<Book> GetBookAsync(Uri uri)
        {
            return await GetAsync<Book>(uri);
        }
        public async Task<House> GetHouseAsync(string uri)
        {
            return await GetAsync<House>(new Uri(uri));
        }
        public async Task<Character> GetCharacterAsync(string uri)
        {
            return await GetAsync<Character>(new Uri(uri));
        }
        private readonly Uri serverUrl = new Uri("https://anapioficeandfire.com/");
        public async Task<List<Book>> GetBooksAsync()
        {
            return await GetAsync<List<Book>>(new Uri(serverUrl, "api/books"));
        }


    }
}
