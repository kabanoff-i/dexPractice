using System.Text;
using TeamService.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace TeamService
{
    public class TeamService
    {
        private string _token;
        public async Task<AuthResponse> Authorize(AuthData data)
        {
            AuthResponse authResponse;
            var serializedData = JsonConvert.SerializeObject(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await
                client.PostAsync("http://dev.trainee.dex-it.ru/api/Auth/SignIn", content);
                var message = await
                responseMessage.Content.ReadAsStringAsync();
                authResponse = JsonConvert.DeserializeObject<AuthResponse>(message);
            }
            _token = authResponse.Token;
            return authResponse;
        }
        public async Task Add(Team team)
        {
            string serialisedTeam = JsonConvert.SerializeObject(team);
            var content = new StringContent(serialisedTeam,
            Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(_token);
                await
                client.PostAsync("http://dev.trainee.dex-it.ru/api/Team/Add", content);
            }
        }
        public async Task<TeamResponse> GetTeams()
        {
            TeamResponse teams;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage responseMessage = await
                client.GetAsync("http://dev.trainee.dex-it.ru/api/Team/GetTeams");
                responseMessage.EnsureSuccessStatusCode();
                string message = await responseMessage.Content.ReadAsStringAsync();
                teams = JsonConvert.DeserializeObject<TeamResponse>(message);
            }
            return teams;
        }
    }

}
