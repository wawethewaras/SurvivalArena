using SurvivalArena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.PoolingSystem {
    public class PoolableComponent {
        public IUpdater updater;

        public PoolableComponent(IUpdater updater) {
            this.updater = updater;
        }

        public void ReleaseSelf() {
            PoolManager.gameObjects.Remove(updater);
        }
    }
}
