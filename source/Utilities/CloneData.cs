﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace Dark.Cloning
{
    /// <summary>
    /// Data class that stores all important data about clones<br />
    /// Has some helper methods for applying its data to pawns and requests
    /// </summary>
    public class CloneData : IExposable
    {
        public CustomXenotype customXenotype;
        public XenotypeDef xenotype;
        public bool UniqueXenotype => customXenotype != null;

        public List<GeneDef> xenogenes => UniqueXenotype ? customXenotype.genes : xenotype.genes;

        public Pawn donorPawn;
        public Gender? fixedGender;
        public HeadTypeDef headType;
        public Color? skinColorOverride;
        public FurDef furDef;
        public HairDef hairDef;
        public BeardDef beardDef;
        public BodyTypeDef bodyType;

        public CloneData() { }
        public CloneData(Pawn donorPawn, CustomXenotype customXenotype)
        {
            this.donorPawn = donorPawn;
            this.customXenotype = customXenotype;

            this.fixedGender = this.donorPawn?.gender;
            this.headType = this.donorPawn?.story?.headType;
            this.skinColorOverride = this.donorPawn?.story?.skinColorOverride;
            this.furDef = this.donorPawn?.story?.furDef;
            this.hairDef = this.donorPawn?.story?.hairDef;
            this.beardDef = this.donorPawn?.style?.beardDef;
            this.bodyType = this.donorPawn?.story?.bodyType;
        }
        public CloneData(Pawn donorPawn, XenotypeDef xenotypeDef)
        {
            this.donorPawn = donorPawn;
            this.xenotype = xenotypeDef;

            this.fixedGender = this.donorPawn?.gender;
            this.headType = this.donorPawn?.story?.headType;
            this.skinColorOverride = this.donorPawn?.story?.skinColorOverride;
            this.furDef = this.donorPawn?.story?.furDef;
            this.hairDef = this.donorPawn?.story?.hairDef;
            this.beardDef = this.donorPawn?.style?.beardDef;
            this.bodyType = this.donorPawn?.story?.bodyType;
        }
        public CloneData(Pawn donorPawn)
        {
            this.donorPawn = donorPawn;

            if (donorPawn.genes.UniqueXenotype)
            {
                //TODO: When unstable hits main, switch to this commented code
                //this.customXenotype = donorPawn.genes.CustomXenotype;
                this.customXenotype = new CustomXenotype();
                this.customXenotype.name = donorPawn.genes.xenotypeName;
                this.customXenotype.iconDef = donorPawn.genes.iconDef;
                this.customXenotype.genes = GeneUtils.GetGenesAsDefs(donorPawn.genes.GenesListForReading);
            }
            else
            {
                this.xenotype = donorPawn.genes.Xenotype;
            }

            this.fixedGender = this.donorPawn?.gender;
            this.headType = this.donorPawn?.story?.headType;
            this.skinColorOverride = this.donorPawn?.story?.skinColorOverride;
            this.furDef = this.donorPawn?.story?.furDef;
            this.hairDef = this.donorPawn?.story?.hairDef;
            this.beardDef = this.donorPawn?.style?.beardDef;
            this.bodyType = this.donorPawn?.story?.bodyType;
        }

        public void ApplyToRequest(ref PawnGenerationRequest request)
        {
            // First copy basic data from the donor 
            request.FixedGender = this.fixedGender;

            request.CanGeneratePawnRelations = false;

            ApplyCloneXenotypeToRequest(ref request);
        }

        public void ApplyCloneXenotypeToRequest(ref PawnGenerationRequest request)
        {
            // Now, add the previously-chosen xenotype to the new pawn
            //request.ForcedXenogenes = cloneData.forcedXenogenes.GenesListForReading;

            //TODO: Overhaul xenotype application when the vanilla xenotype changes roll out
            if (this.UniqueXenotype)
            {
                request.ForcedCustomXenotype = this.customXenotype;
            }
            else
            {
                request.ForcedXenotype = this.xenotype;
            }
        }
        public void ApplyAppearance(Pawn pawn)
        {
            if (pawn == null)
            {
                Log.Error("Error applying clone appearance, pawn was null");
                return;
            }
            //TODO: Add checks for ideology if necessary, like for beards
            pawn.story.headType = this.headType;
            pawn.story.skinColorOverride = this.skinColorOverride;
            pawn.story.furDef = this.furDef;
            if (CloningSettings.inheritHair)
            {
                pawn.story.hairDef = this.hairDef;
                pawn.style.beardDef = this.beardDef;
            }
            pawn.style.Notify_StyleItemChanged();
        }

        public void ApplyBodyType(Pawn pawn)
        {
            pawn.story.bodyType = this.bodyType ?? pawn.story.bodyType; // Null coalesce in case the clonegene doesn't contain valid bodytype data for some reason
            pawn.style.Notify_StyleItemChanged();
        }

        public void ExposeData()
        {
            Scribe_Deep.Look(ref this.customXenotype, "customXenotype");
            Scribe_Defs.Look(ref xenotype, "xenotype");
            
            Scribe_References.Look(ref this.donorPawn, "donorPawn");
            Scribe_Values.Look(ref this.fixedGender, "fixedGender");
            Scribe_Values.Look(ref this.skinColorOverride, "skinColorOverride");
            Scribe_Defs.Look(ref this.headType, "headType");
            Scribe_Defs.Look(ref this.furDef, "furDef");
            Scribe_Defs.Look(ref this.hairDef, "hairDef");
            Scribe_Defs.Look(ref this.beardDef, "beardDef");
            Scribe_Defs.Look(ref this.bodyType, "bodyType");
        }
    }
}
