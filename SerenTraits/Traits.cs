using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using UnityEngine;
using static CursedProdigy.CustomFunctions;
using static CursedProdigy.Plugin;

namespace CursedProdigy
{
    [HarmonyPatch]
    internal class Traits
    {
        // list of your trait IDs
        public static string heroName = "Seren";

        public static string subclassname = "cursedprodigy";

        public static string[] simpleTraitList = ["trait0", "trait1a", "trait1b", "trait2a", "trait2b", "trait3a", "trait3b", "trait4a", "trait4b"];

        public static string[] myTraitList = simpleTraitList.Select(trait => subclassname + trait).ToArray(); // Needs testing

        static string trait0 = myTraitList[0];
        static string trait2a = myTraitList[3];
        static string trait2b = myTraitList[4];
        static string trait4a = myTraitList[7];
        static string trait4b = myTraitList[8];
        public static int damageMultiplier = 0;
        public static int firstCurseDamage = 0;
        public static HashSet<Enums.CardType> empoweredTypes = [Enums.CardType.Fire_Spell, Enums.CardType.Cold_Spell, Enums.CardType.Lightning_Spell];
        // public static HashSet<Enums.CardType> currentTypes = [Enums.CardType.Fire_Spell, Enums.CardType.Cold_Spell, Enums.CardType.Lightning_Spell];


        public static string debugBase = "Binbin - Testing " + heroName + " ";


        public static void DoCustomTrait(string _trait, ref Trait __instance)
        {
            // get info you may need
            Enums.EventActivation _theEvent = Traverse.Create(__instance).Field("theEvent").GetValue<Enums.EventActivation>();
            Character _character = Traverse.Create(__instance).Field("character").GetValue<Character>();
            Character _target = Traverse.Create(__instance).Field("target").GetValue<Character>();
            int _auxInt = Traverse.Create(__instance).Field("auxInt").GetValue<int>();
            string _auxString = Traverse.Create(__instance).Field("auxString").GetValue<string>();
            CardData _castedCard = Traverse.Create(__instance).Field("castedCard").GetValue<CardData>();
            Traverse.Create(__instance).Field("character").SetValue(_character);
            Traverse.Create(__instance).Field("target").SetValue(_target);
            Traverse.Create(__instance).Field("theEvent").SetValue(_theEvent);
            Traverse.Create(__instance).Field("auxInt").SetValue(_auxInt);
            Traverse.Create(__instance).Field("auxString").SetValue(_auxString);
            Traverse.Create(__instance).Field("castedCard").SetValue(_castedCard);
            TraitData traitData = Globals.Instance.GetTraitData(_trait);
            List<CardData> cardDataList = [];
            List<string> heroHand = MatchManager.Instance.GetHeroHand(_character.HeroIndex);
            Hero[] teamHero = MatchManager.Instance.GetTeamHero();
            NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();

            if (_trait == trait0)
            { // Max Powerful Charges +10. Immune to Bless.
                // done in GACM
                string traitName = traitData.TraitName;
                string traitId = _trait;
                LogDebug($"Handling Trait {traitId}: {traitName}");
            }


            else if (_trait == trait2a)
            { // trait 2a: Gain +1 Burn, Chill, and Spark for every 2 Curse Spells in your deck.",
                string traitName = traitData.TraitName;
                string traitId = _trait;
                LogDebug($"Handling Trait {traitId}: {traitName}");
                // DisplayTraitScroll(ref _character, traitData);

            }



            else if (_trait == trait2b)
            { // trait 2b: At the start of your turn, increase the cost of all of your cards by 1 until discarded. Then half the cost of the highest cost Fire Spell in your hand that costs 6 or more. Repeat for Cold, Lighting, Shadow, and Curse Spells.
                string traitName = traitData.TraitName;
                string traitId = _trait;
                LogDebug($"Handling Trait {traitId}: {traitName}");
                if (!AtOManager.Instance.TeamHaveTrait(trait4a) && IsLivingHero(_character))
                {
                    for (int i = 0; i < heroHand.Count; i++)
                    {
                        CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
                        ReduceCardCost(ref cardData, _character, -1);
                    }
                }

                Enums.CardType[] cardTypes = [Enums.CardType.Fire_Spell, Enums.CardType.Cold_Spell, Enums.CardType.Lightning_Spell, Enums.CardType.Shadow_Spell, Enums.CardType.Curse_Spell];
                foreach (Enums.CardType cardType in cardTypes)
                {
                    CardData highestCostCard = GetRandomHighestCostCard(cardType, heroHand);
                    int energy = highestCostCard.EnergyCost - highestCostCard.EnergyReductionPermanent - highestCostCard.EnergyReductionTemporal;
                    if (highestCostCard != null && energy >= 6 && IsLivingHero(_character))
                    {
                        int amountToReduce = Mathf.FloorToInt(energy / 2);
                        ReduceCardCost(ref highestCostCard, _character, amountToReduce);
                    }
                }

                // DisplayTraitScroll(ref _character, traitData);

            }

            else if (_trait == trait4a)
            { // The first Curse you play each turn deals triple damage. Cursed Elements gives +2 Elemental Charges for every 3 Curse Spells in your deck. Sorcerous Mastery no longer increases the cost.
                // done in GetTraitAuraCurseModifiersPostfix and GetTraitDamagePercentModifiersPostfix
                string traitName = traitData.TraitName;
                string traitId = _trait;
                LogDebug($"Handling Trait {traitId}: {traitName}");
                if (CanIncrementTraitActivations(traitId) && _castedCard != null && _castedCard.HasCardType(Enums.CardType.Curse_Spell))
                {
                    firstCurseDamage = 200;
                    IncrementTraitActivations(traitId);
                }
                else
                {
                    firstCurseDamage = 0;
                }

            }

            else if (_trait == trait4b)
            { // Fire Empowers Cold, Cold Empowers Lightning, Lightning Empowers Fire. Empowered Spells deal 30% bonus damage that is increased by 30% for each consecutively played Empowered Spell this turn.
                string traitName = traitData.TraitName;
                string traitId = _trait;
                LogDebug($"Handling Trait {traitId}: {traitName}");
                HashSet<Enums.CardType> cardSet = [.. _castedCard.CardTypeAux];
                cardSet.Add(_castedCard.CardType);
                cardSet.IntersectWith(empoweredTypes);
                if (cardSet.Count > 0)
                {
                    damageMultiplier += 30;
                    empoweredTypes = [];
                    foreach (Enums.CardType cardType in cardSet)
                    {
                        empoweredTypes.Add(GetEmpoweredType(cardType));
                    }
                }
                else
                {
                    // empoweredTypes = [];
                    empoweredTypes = [Enums.CardType.Fire_Spell, Enums.CardType.Cold_Spell, Enums.CardType.Lightning_Spell];
                    damageMultiplier = 0;
                }

            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Trait), "DoTrait")]
        public static bool DoTrait(Enums.EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard, ref Trait __instance)
        {
            if ((UnityEngine.Object)MatchManager.Instance == (UnityEngine.Object)null)
                return false;
            Traverse.Create(__instance).Field("character").SetValue(_character);
            Traverse.Create(__instance).Field("target").SetValue(_target);
            Traverse.Create(__instance).Field("theEvent").SetValue(_theEvent);
            Traverse.Create(__instance).Field("auxInt").SetValue(_auxInt);
            Traverse.Create(__instance).Field("auxString").SetValue(_auxString);
            Traverse.Create(__instance).Field("castedCard").SetValue(_castedCard);
            if (Content.medsCustomTraitsSource.Contains(_trait) && myTraitList.Contains(_trait))
            {
                DoCustomTrait(_trait, ref __instance);
                return false;
            }
            return true;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.GetTraitDamagePercentModifiers))]
        public static void GetTraitDamagePercentModifiersPostfix(ref Character __instance, ref float __result, Enums.DamageType DamageType, bool ___useCache)
        {
            LogInfo("GetTraitDamagePercentModifiersPostfix");
            // Fire Empowers Cold, Cold Empowers Lightning, Lightning Empowers Fire. Empowered Spells deal 30% bonus damage that is increased by 30% for each consecutively played Empowered Spell.
            string traitOfInterest = trait4b;

            if (IsLivingHero(__instance) && AtOManager.Instance != null && AtOManager.Instance.CharacterHaveTrait(__instance.SubclassName, traitOfInterest) && MatchManager.Instance != null)
            {
                __result += damageMultiplier;
            }

            // The first Curse you play each turn deals +200% damage
            traitOfInterest = trait4a;
            if (IsLivingHero(__instance) && AtOManager.Instance != null && AtOManager.Instance.CharacterHaveTrait(__instance.SubclassName, traitOfInterest) && MatchManager.Instance != null)
            {

                __result += firstCurseDamage;
            }
        }



        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {
            LogInfo($"GACM {subclassName}");

            Character characterOfInterest = _type == "set" ? _characterTarget : _characterCaster;
            string traitOfInterest;

            switch (_acId)
            {
                case "powerful":
                    traitOfInterest = trait0;
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.ThisHero))
                    {
                        __result.MaxCharges += 5;
                        __result.MaxMadnessCharges += 5;
                    }
                    string itemId = "cursedprodigycursedwandrare";
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Item, itemId, AppliesTo.Heroes))
                    {
                        __result.MaxCharges += 5;
                        __result.MaxMadnessCharges += 5;
                    }
                    break;
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.GetTraitAuraCurseModifiers))]
        public static void GetTraitAuraCurseModifiersPostfix(ref Character __instance, ref Dictionary<string, int> __result)
        {
            LogDebug("GetTraitAuraCurseModifiersPostfix");
            string traitOfInterest = trait4a;
            if (isDamagePreviewActive|| isCalculateDamageActive || !IsLivingHero(__instance) || AtOManager.Instance == null || !AtOManager.Instance.CharacterHaveTrait(__instance.SubclassName, trait2a) || MatchManager.Instance == null)
            {
                return;
            }


            int nCurses = GetDeck(__instance).Count(card => Globals.Instance.GetCardData(card).HasCardType(Enums.CardType.Curse_Spell));
            int nToIncrease = !AtOManager.Instance.TeamHaveTrait(traitOfInterest) ? Mathf.FloorToInt(nCurses / 2) : Mathf.FloorToInt(2 * nCurses / 3);
            // int nToIncrease = Mathf.FloorToInt(nInsane * 0.1f);
            if (nToIncrease <= 0)
            {
                return;
            }
            if (!__result.ContainsKey("burn"))
            {
                __result["burn"] = 0;
            }
            __result["burn"] = nToIncrease;

            if (!__result.ContainsKey("chill"))
            {
                __result["chill"] = 0;
            }
            __result["chill"] = nToIncrease;

            if (!__result.ContainsKey("spark"))
            {
                __result["spark"] = 0;
            }
            __result["spark"] = nToIncrease;
        }


        public static Enums.CardType GetEmpoweredType(Enums.CardType cardType)
        {
            switch (cardType)
            {
                case Enums.CardType.Fire_Spell:
                    return Enums.CardType.Cold_Spell;
                case Enums.CardType.Cold_Spell:
                    return Enums.CardType.Lightning_Spell;
                case Enums.CardType.Lightning_Spell:
                    return Enums.CardType.Fire_Spell;
                default:
                    return Enums.CardType.Fire_Spell;
            }
        }

        public static bool isDamagePreviewActive;
        public static bool isCalculateDamageActive;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), nameof(MatchManager.SetDamagePreview))]
        public static void SetDamagePreviewPrefix()
        {
            isDamagePreviewActive = true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MatchManager), nameof(MatchManager.SetDamagePreview))]
        public static void SetDamagePreviewPostfix()
        {
            isDamagePreviewActive = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterItem), nameof(CharacterItem.CalculateDamagePrePostForThisCharacter))]
        public static void CalculateDamagePrePostForThisCharacterPrefix()
        {
            isCalculateDamageActive = true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharacterItem), nameof(CharacterItem.CalculateDamagePrePostForThisCharacter))]
        public static void CalculateDamagePrePostForThisCharacterPostfix()
        {
            isCalculateDamageActive = false;
        }

    }
}
