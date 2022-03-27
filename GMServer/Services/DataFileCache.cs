﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GMServer.Services
{
    internal class DataFileCachedObject
    {
        public string File;
        public DateTime LoadedAt;
        public string Text;
    }

    public interface IDataFileCache
    {
        public T Load<T>(string fp);
    }

    public static class DataFiles
    {
        public const string Mercs = "Datafiles/Mercs.json";
        public const string Artefacts = "DataFiles/Artefacts.json";
        public const string Armoury = "Datafiles/Armoury.json";
        public const string Bounties = "Datafiles/Bounties.json";
        public const string BountyShop = "Datafiles/BountyShop.json";
        public const string Quests = "Datafiles/Quests.json";
    }


    public class DataFileCache : IDataFileCache
    {
        private readonly Dictionary<string, DataFileCachedObject> _cache;
        private readonly long CacheInterval = 60 * 15;

        public DataFileCache()
        {
            _cache = new Dictionary<string, DataFileCachedObject>();
        }

        public T Load<T>(string fp)
        {
            return JsonConvert.DeserializeObject<T>(LoadOrCache(fp));
        }

        private string LoadOrCache(string fp)
        {
            if (!_cache.TryGetValue(fp, out DataFileCachedObject cachedObject))
            {
                cachedObject = new() { File = fp, LoadedAt = DateTime.UtcNow, Text = LoadFile(fp) };

                _cache.Add(fp, cachedObject);
            }

            if (IsOutdated(cachedObject))
                ReloadCachedItem(ref cachedObject);

            return cachedObject.Text;
        }

        private void ReloadCachedItem(ref DataFileCachedObject cachedObject)
        {
            cachedObject.LoadedAt = DateTime.UtcNow;
            cachedObject.Text = LoadFile(cachedObject.File);
        }

        private string LoadFile(string fp)
        {
            return System.IO.File.ReadAllText(fp);
        }

        private bool IsOutdated(DataFileCachedObject cachedObject)
        {
            long nowTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            long lastLoadTimestamp = new DateTimeOffset(cachedObject.LoadedAt).ToUnixTimeSeconds();

            return (nowTimestamp / CacheInterval) != (lastLoadTimestamp / CacheInterval);
        }
    }
}
