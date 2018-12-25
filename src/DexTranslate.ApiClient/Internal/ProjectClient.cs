using DexTranslate.ApiContract.v1;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DexTranslate.ApiClient.Internal
{
    internal class ProjectClient : BaseDexApiClient
    {
        public ProjectClient(HttpClient client, string baseUrl) : base(client, baseUrl)
        {
        }

        protected override string Url => BaseUrl + nameof(Project);

        public async Task AddProject(Project project)
        {
            var response = await Client.PostAsync(Url, GetJsonContent(project));
            await EnsureSuccess(response);
        }

        public async Task UpdateProject(Project project)
        {
            var response = await Client.PutAsync(Url, GetJsonContent(project));
            await EnsureSuccess(response);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            string json = await Client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Project>>(json);
        }

        public async Task DeleteProject(string key)
        {
            var response = await Client.DeleteAsync(Url + "/" + key);
            await EnsureSuccess(response);
        }
    }
}