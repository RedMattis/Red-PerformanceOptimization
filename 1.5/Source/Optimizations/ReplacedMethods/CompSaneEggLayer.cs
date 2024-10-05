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
        public PropertyInfo activeInfo = null;
        public bool Active
        {
            get
            {
                if (activeInfo == null)
                {
                    activeInfo = typeof(CompEggLayer).GetProperty("Active", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return (bool)activeInfo.GetValue(this);
            }
        }

        public FieldInfo eggProgressInfo = null;
        public float EggProgress
        {
            get
            {
                if (eggProgressInfo == null)
                {
                    eggProgressInfo = typeof(CompEggLayer).GetField("eggProgress", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return (float)eggProgressInfo.GetValue(this);
            }
            set
            {
                if (eggProgressInfo == null)
                {
                    eggProgressInfo = typeof(CompEggLayer).GetField("eggProgress", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                eggProgressInfo.SetValue(this, value);
            }
        }

        public PropertyInfo progressStoppedBecauseUnfertilizedInfo = null;
        public bool ProgressStoppedBecauseUnfertilized
        {
            get
            {
                if (progressStoppedBecauseUnfertilizedInfo == null)
                {
                    progressStoppedBecauseUnfertilizedInfo = typeof(CompEggLayer).GetProperty("ProgressStoppedBecauseUnfertilized", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return (bool)progressStoppedBecauseUnfertilizedInfo.GetValue(this);
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
