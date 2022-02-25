﻿using API.Models;
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
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IConfiguration Config { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var result = await HttpClient.GetAsync(Config["apiUrl"] + "/api/CoffeeShop");
            if(result.IsSuccessStatusCode)
            {
                Shops = await result.Content.ReadFromJsonAsync<List<CoffeeShopModel>>();
            }
        }
    }
}
