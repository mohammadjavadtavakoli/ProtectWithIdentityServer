using API.Models;
using Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Client.Pages
{
    public partial class CoffeeShops
    {
        private List<CoffeeShopModel> Shops = new();
        [Inject] private HttpClient _HttpClient { get; set; }
        [Inject] private IConfiguration Config { get; set; }
        [Inject] private ITokenService TokenService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var tokenResponce = await TokenService.GetToken("CofeeAPI.read");

            _HttpClient.SetBearerToken(tokenResponce.AccessToken);

            var result = await _HttpClient.GetAsync(Config["apiUrl"] + "/api/CoffeeShop");
            if (result.IsSuccessStatusCode)
            {
                Shops = await result.Content.ReadFromJsonAsync<List<CoffeeShopModel>>();
            }
        }
    }
}
