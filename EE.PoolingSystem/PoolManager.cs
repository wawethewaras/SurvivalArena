using SurvivalArena;
using SurvivalArena.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.PoolingSystem {
    public class PoolManager {
        public static List<IUpdater> gameObjects = new List<IUpdater>();
    }
}
