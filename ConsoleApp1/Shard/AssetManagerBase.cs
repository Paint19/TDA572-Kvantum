using System;

namespace Shard
{
    abstract class AssetManagerBase
    {
        private String assetPath;

        public string AssetPath { get; set; }

        public abstract void registerAssets();
        public abstract string getAssetPath(string asset);
    }

}
