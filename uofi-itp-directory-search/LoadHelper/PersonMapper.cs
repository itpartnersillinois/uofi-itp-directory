﻿using System.Net;
using System.Text;

namespace uofi_itp_directory_search.LoadHelper {

    public class PersonMapper(string? mapUrl, Action<string> logger) {

        private const string _map = "{ \"settings\": { \"analysis\": { \"filter\": { \"english_stop\": { \"type\": \"stop\", \"stopwords\": \"_english_\" }, \"english_stemmer\": { \"type\": \"stemmer\", \"language\": \"english\" }, \"english_possessive_stemmer\": { \"type\": \"stemmer\", \"language\": \"possessive_english\" } }, \"analyzer\": { \"english\": { \"tokenizer\": \"standard\", \"filter\": [ \"lowercase\", \"english_possessive_stemmer\", \"english_stop\", \"english_stemmer\" ] } } } }, " +
                "\"mappings\" : { \"properties\" : { " +
                "\"id\" : {\"type\": \"keyword\" }, " +
                "\"netid\" : {\"type\": \"keyword\" }, " +
                "\"source\" : {\"type\": \"keyword\" }, " +
                "\"fullname\" : { \"type\" : \"keyword\" }, " +
                "\"fullnamereversed\" : { \"type\" : \"keyword\" }, " +
                "\"firstname\" : { \"type\" : \"keyword\" }, " +
                "\"lastname\" : { \"type\" : \"keyword\" }, " +
                "\"linkname\" : { \"type\" : \"keyword\" }, " +
                "\"preferredpronouns\" : { \"type\" : \"keyword\" }, " +
                "\"email\" : { \"type\" : \"keyword\" }, " +
                "\"addressline1\" : {\"type\": \"keyword\" }, " +
                "\"addressline2\" : {\"type\": \"keyword\" }, " +
                "\"city\" : { \"type\" : \"keyword\" }, " +
                "\"state\" : { \"type\" : \"keyword\" }, " +
                "\"zip\" : { \"type\" : \"keyword\" }, " +
                "\"uin\" : { \"type\" : \"keyword\" }, " +
                "\"twittername\" : { \"type\" : \"keyword\" }, " +
                "\"linkedinurl\" : { \"type\" : \"keyword\" }, " +
                "\"experturl\" : { \"type\" : \"keyword\" }, " +
                "\"profileurl\" : { \"type\" : \"keyword\" }, " +
                "\"imageurl\" : { \"type\" : \"keyword\" }, " +
                "\"cvurl\" : { \"type\" : \"keyword\" }, " +
                "\"quote\" : { \"type\" : \"keyword\" }, " +
                "\"building\" : { \"type\" : \"keyword\" }, " +
                "\"roomnumber\" : { \"type\" : \"keyword\" }, " +
                "\"phone\" : { \"type\" : \"keyword\" }, " +
                "\"hours\" : { \"type\" : \"keyword\" }, " +
                "\"lastupdated\" : { \"type\" : \"date\" }, " +
                "\"jobtypelist\" : { \"type\" : \"keyword\" }, " +
                "\"officelist\" : { \"type\" : \"keyword\" }, " +
                "\"officejobtypelist\" : { \"type\" : \"keyword\" }, " +
                "\"order\" : { \"type\" : \"keyword\" }, " +
                "\"suggest\" : {\"type\": \"completion\", \"contexts\": [ { \"name\": \"source\", \"type\": \"category\" } ] }, " +
                "\"biography\" : { \"type\" : \"text\", \"analyzer\": \"english\" }, " +
                "\"researchstatement\" : { \"type\" : \"text\", \"analyzer\": \"english\" }, " +
                "\"teachingstatement\" : { \"type\" : \"text\", \"analyzer\": \"english\" } " +
                "} } }";

        private readonly Action<string> _logger = logger;
        private readonly bool _logOnly = string.IsNullOrWhiteSpace(mapUrl);
        private readonly string _mapUrl = (mapUrl?.TrimEnd('/') ?? "") + "/dr_person";

        public async Task<string> Map() {
            _logger($"{_mapUrl}: PUT {(_logOnly ? _map : "")}");
            if (_logOnly) {
                return "";
            }
            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage {
                Version = HttpVersion.Version10,
                Content = new StringContent(_map, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_mapUrl),
                Method = HttpMethod.Put
            }).ConfigureAwait(continueOnCapturedContext: false);
            return await response.Content.ReadAsStringAsync();
        }
    }
}