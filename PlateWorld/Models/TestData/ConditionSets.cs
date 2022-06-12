﻿using PlateWorld.Models.BasicTypes;
using System.Collections.Generic;
using System.Linq;

namespace PlateWorld.Models.TestData
{
    public static class ConditionSets
    {
        public static IEnumerable<IConditionSet> TestConditionSets
        {
            get 
            {
                yield return MakePhs();
                yield return MakeDillutions();
                yield return MakeReplicateNumbers();
                yield return MakeEnzymeAssayBuffers();
                yield return MakeCypEnzymes();
                yield return MakeCypSubstrates();
            }
        }

        public static IConditionSet MakePhs()
        {
            var tvs = new[] { 4.0, 4.5, 5.0, 5.5, 6.0 };
            return new ConditionSet(
                name: "Ph 4-6",
                description: "5 steps between 4 and 6",
                propertyType: PropertyTypeExt.MakePh(),
                testValues: tvs.Select(v => (object)v));
        }

        public static IConditionSet MakeDillutions()
        {
            var tvs = new[] { 0, 1, 2, 3, 4 };
            return new ConditionSet(
                name: "5 Dilutions",
                description: "5 Dillutions from Log(0) and Log(4)",
                propertyType: PropertyTypeExt.MakeDillution(),
                testValues: tvs.Select(v => (object)v));
        }

        public static IConditionSet MakeReplicateNumbers()
        {
            var tvs = new[] { 0, 1, 2, 3, 4 };
            return new ConditionSet(
                name: "3 Replicates",
                description: "Replica numbers 1, 2, and 3",
                propertyType: PropertyTypeExt.MakeReplicateNumber(),
                testValues: tvs.Select(v => (object)v));
        }

        public static IConditionSet MakeEnzymeAssayBuffers()
        {
            var tvs = new[] { "MOPS", "HEPES", "Tris-HCl" };
            return new ConditionSet(
                name: "Enzyme Assay Buffers",
                description: "MOPS, HEPES, or Tris-HCl",
                propertyType: PropertyTypeExt.MakeBuffer(),
                testValues: tvs.Select(v => (object)v));
        }

        public static IConditionSet MakeCypEnzymes()
        {
            var tvs = new[] { "CYP450", "CYP5", "CYP8" };
            return new ConditionSet(
                name: "Cyp Enzymes",
                description: "CYP450, CYP5, or CYP8",
                propertyType: PropertyTypeExt.MakeEnzyme(),
                testValues: tvs.Select(v => (object)v));
        }

        public static IConditionSet MakeCypSubstrates()
        {
            var tvs = new[] { "bilrubin", "vitamin D", "cholesterol", "paracetamol" };
            return new ConditionSet(
                name: "Cyp Substrates",
                description: "bilrubin, vitamin D, cholesterol , or paracetamol",
                propertyType: PropertyTypeExt.MakeSubstrate(),
                testValues: tvs.Select(v => (object)v));
        }

    }
}
