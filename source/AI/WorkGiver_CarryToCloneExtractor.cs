﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Dark.Cloning
{
	public class WorkGiver_CarryToCloneExtractor : WorkGiver_CarryToBuilding
	{
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(CloneDefOf.CloneExtractor);

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}