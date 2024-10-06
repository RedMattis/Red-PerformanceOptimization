using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RedOpt
{
    public class CompProperties_SaneEggLayer : CompProperties_EggLayer
    {
        public CompProperties_SaneEggLayer()
        {
            compClass = typeof(CompSaneEggLayer);
        }
    }

    public class CompSaneEggLayer : CompEggLayer
    {
        public CompProperties_SaneEggLayer SaneProps => (CompProperties_SaneEggLayer)props;
        public Func<CompEggLayer, bool> activeDelegate = null;
        public bool Active
        {
            get
            {
                activeDelegate ??= AccessTools.MethodDelegate<Func<CompEggLayer, bool>>(AccessTools.PropertyGetter(typeof(CompEggLayer), "Active"));
                return activeDelegate(this);
            }
        }

        public AccessTools.FieldRef<CompEggLayer, float> eggProgressDelegate = null;
        public float EggProgress
        {
            get
            {
                eggProgressDelegate ??= AccessTools.FieldRefAccess<CompEggLayer, float>("eggProgress");
                return eggProgressDelegate(this);
            }
            set
            {
                eggProgressDelegate ??= AccessTools.FieldRefAccess<CompEggLayer, float>("eggProgress");
                eggProgressDelegate(this) = value;
            }
        }

        public Func<CompEggLayer, bool> progressStoppedBecauseUnfertilizedDegegate = null;
        public bool ProgressStoppedBecauseUnfertilized
        {
            get
            {
                progressStoppedBecauseUnfertilizedDegegate ??= AccessTools.MethodDelegate<Func<CompEggLayer, bool>>(AccessTools.PropertyGetter(typeof(CompEggLayer), "ProgressStoppedBecauseUnfertilized"));
                return progressStoppedBecauseUnfertilizedDegegate(this);
            }
        }

        public override void CompTick()
        {
            const int tickRate = 1000;
            const int ticksPerDay = 60000;
            int tick = Find.TickManager.TicksGame;
            if (tick % tickRate == 0)
            {
                if (Active)
                {
                    float num = tickRate / (Props.eggLayIntervalDays * ticksPerDay);
                    if (parent is Pawn pawn)
                    {
                        num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
                    }
                    EggProgress += num;
                    if (EggProgress > 1f)
                    {
                        EggProgress = 1f;
                    }
                    if (ProgressStoppedBecauseUnfertilized)
                    {
                        EggProgress = Props.eggProgressUnfertilizedMax;
                    }
                }
            }
        }
    }
}
