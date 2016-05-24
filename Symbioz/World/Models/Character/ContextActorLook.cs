using Symbioz.DofusProtocol.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Symbioz.Helper;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Records;
using Symbioz.Enums;

namespace Symbioz.World.Models
{
    public class ContextActorLook : EntityLook
    {
        public const short AURA_SCALE = 100;
        public bool IsRiding { get { return subentities.Find(x=>x.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER) != null;} }
        public ContextActorLook() { }
        public ContextActorLook(ushort bonesid,List<ushort> skins,List<int> colors,List<short> scales,List<SubEntity> subentities):base(bonesid,skins,colors,scales,subentities)
        {
           
        }
        public SubEntity RiderSubEntity()
        {
            return subentities.Find(x => x.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER);
        }
        public ContextActorLook CloneContextActorLook()
        {
            return new ContextActorLook(bonesId, new List<ushort>(skins), new List<int>(indexedColors),new List<short>(scales), new List<SubEntity>(subentities.ConvertAll<SubEntity>(x=>CloneSubEntity(x))));
        }
        public SubEntity CloneSubEntity(SubEntity subentity)
        {
            return new SubEntity(subentity.bindingPointCategory, subentity.bindingPointIndex, new EntityLook(subentity.subEntityLook.bonesId, new List<ushort>(subentity.subEntityLook.skins),
                new List<int>(subentity.subEntityLook.indexedColors), new List<short>(subentity.subEntityLook.scales), subentity.subEntityLook.subentities)); // subentity.sublook.subentites not cloned
        }
        public EntityLook ToEntityLook()
        {
            return new EntityLook(bonesId, skins, indexedColors, scales, subentities);
        }
        public void SetScale(short scale)
        {
            if (IsRiding)
                RiderSubEntity().subEntityLook.scales[0] = scale;
            else
                scales[0] = scale;
        }
        public void SetBonesId(ushort id)
        {
            if (IsRiding)
                RiderSubEntity().subEntityLook.bonesId = id;
            else
                bonesId = id;
        }
        public void RemoveSkin(ushort id)
        {
            if (IsRiding)
                RiderSubEntity().subEntityLook.skins.Remove(id);
            else
                skins.Remove(id);
        }
        public void AddSkin(ushort id)
        {
            if (IsRiding)
            {
                var look = RiderSubEntity().subEntityLook;
                if (!look.skins.Contains(id))
                look.skins.Add(id);
            }
            else
                skins.Add(id);
        }
        public void UnsetAura()
        {
            subentities.RemoveAll(x => x.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_BASE_FOREGROUND);
        }
        public void SetAura(ushort bonesid)
        {
            subentities.Add(new SubEntity((sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_BASE_FOREGROUND, 0, SimpleBonesLook(bonesid,AURA_SCALE)));
        }
        public ContextActorLook CharacterToRider(ushort bonesid, List<ushort> rskins, List<int> rcolors, short rscale)
        {
            this.bonesId = 2;
            ContextActorLook newLook = new ContextActorLook(bonesid, rskins, rcolors, new List<short>() { rscale }, new List<SubEntity>() { new SubEntity((sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER, 0, ToEntityLook()) });
            return newLook;
        }
        public ContextActorLook RiderToCharacter()
        {
            var playerLook = this.subentities.Find(x => x.bindingPointCategory == (sbyte)SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER);
            if (playerLook != null)
            {
                playerLook.subEntityLook.bonesId = 1;
                return ContextActorLook.FromEntityLook(playerLook.subEntityLook);
            }
            else return this;
        }
        public ContextActorLook CharacterToRider(ushort bonesid, short rscale)
        {
            return CharacterToRider(bonesid, new List<ushort>(), new List<int>(), rscale);
        }
        public static ContextActorLook Parse(string str)
        {
            if (string.IsNullOrEmpty(str) || str[0] != '{')
            {
                throw new System.Exception("Incorrect EntityLook format : " + str);
            }
            int i = 1;
            int num = str.IndexOf('|');
            if (num == -1)
            {
                num = str.IndexOf("}");
                if (num == -1)
                {
                    throw new System.Exception("Incorrect EntityLook format : " + str);
                }
            }
            short bones = short.Parse(str.Substring(i, num - i));
            i = num + 1;
            short[] skins = new short[0];
            if ((num = str.IndexOf('|', i)) != -1 || (num = str.IndexOf('}', i)) != -1)
            {
                skins = Extensions.ParseCollection<short>(str.Substring(i, num - i), new Func<string, short>(short.Parse));
                i = num + 1;
            }
            Tuple<int, int>[] source = new Tuple<int, int>[0];
            if ((num = str.IndexOf('|', i)) != -1 || (num = str.IndexOf('}', i)) != -1)
            {
                source = Extensions.ParseCollection<Tuple<int, int>>(str.Substring(i, num - i), new Func<string, Tuple<int, int>>(ParseIndexedColor));
                i = num + 1;
            }
            short[] scales = new short[0];
            if ((num = str.IndexOf('|', i)) != -1 || (num = str.IndexOf('}', i)) != -1)
            {
                scales = Extensions.ParseCollection<short>(str.Substring(i, num - i), new Func<string, short>(short.Parse));
                i = num + 1;
            }
            System.Collections.Generic.List<SubEntity> list = new System.Collections.Generic.List<SubEntity>();
            while (i < str.Length)
            {
                int num2 = str.IndexOf('@', i, 3);
                int num3 = str.IndexOf('=', num2 + 1, 3);
                byte category = byte.Parse(str.Substring(i, num2 - i));
                byte b = byte.Parse(str.Substring(num2 + 1, num3 - (num2 + 1)));
                int num4 = 0;
                int num5 = num3 + 1;
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                do
                {
                    stringBuilder.Append(str[num5]);
                    if (str[num5] == '{')
                    {
                        num4++;
                    }
                    else
                    {
                        if (str[num5] == '}')
                        {
                            num4--;
                        }
                    }
                    num5++;
                }
                while (num4 > 0);
                list.Add(new SubEntity((sbyte)category, (sbyte)b, Parse(stringBuilder.ToString())));
                i = num5 + 1;
            }
            List<int> colors = new List<int>();
            foreach (var color in source)
            {
                colors.Add(color.Item2);
            }
            return new ContextActorLook((ushort)bones, skins.Select(entry => (ushort)entry).ToList(), colors.ToList(), scales.ToList(), list);
        }
        private static Tuple<int, int> ParseIndexedColor(string str)
        {
            int num = str.IndexOf("=");
            bool flag = str[num + 1] == '#';
            int item = int.Parse(str.Substring(0, num));
            int item2 = int.Parse(str.Substring(num + (flag ? 2 : 1), str.Length - (num + (flag ? 2 : 1))), flag ? System.Globalization.NumberStyles.HexNumber : System.Globalization.NumberStyles.Integer);
            return Tuple.Create<int, int>(item, item2);
        }
        public static string ConvertToString(EntityLook look)
        {
            return new ContextActorLook(look.bonesId, look.skins, look.indexedColors, look.scales, look.subentities).ConvertToString();
        }
        public string ConvertToString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("{");
            int num = 0;
            stringBuilder.Append(bonesId);
            if (skins == null || !skins.Any<ushort>())
            {
                num++;
            }
            else
            {
                stringBuilder.Append("|".ConcatCopy(num + 1));
                num = 0;
                stringBuilder.Append(string.Join<ushort>(",", skins));
            }
            if (indexedColors == null)
            {
                num++;
            }
            else
            {
                stringBuilder.Append("|".ConcatCopy(num + 1));
                num = 0;
                stringBuilder.Append(string.Join(",",
                    from entry in indexedColors
                    select "1" + "=" + entry));
            }
            if (scales == null)
            {
                num++;
            }
            else
            {
                stringBuilder.Append("|".ConcatCopy(num + 1));
                num = 0;
                stringBuilder.Append(string.Join<short>(",", scales));
            }
            if (subentities.Count() == 0)
            {
                num++;
            }
            else
            {
                List<string> subEntitiesAsString = new List<string>();
                foreach (var sub in subentities)
                {
                    StringBuilder subBuilter = new System.Text.StringBuilder();
                    subBuilter.Append((sbyte)sub.bindingPointCategory);
                    subBuilter.Append("@");
                    subBuilter.Append(sub.bindingPointIndex);
                    subBuilter.Append("=");
                    subBuilter.Append(ConvertToString(sub.subEntityLook));
                    subEntitiesAsString.Add(subBuilter.ToString());
                }
                stringBuilder.Append("|".ConcatCopy(num + 1));
                stringBuilder.Append(string.Join<string>(",",
                    from entry in subEntitiesAsString
                    select entry));
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
     
        public static ContextActorLook FromEntityLook(EntityLook look)
        {
            return new ContextActorLook(look.bonesId, look.skins, look.indexedColors, look.scales, look.subentities);
        }
        public static ContextActorLook SimpleBonesLook(ushort bonesid,short scale = 100)
        {
            return new ContextActorLook(bonesid,new List<ushort>(),new List<int>(),new List<short>(){scale},new List<SubEntity>());
        }
        public static ContextActorLook SimpleSkinLook(ushort skinid, short scale = 100)
        {
            return new ContextActorLook(1, new List<ushort>(){skinid}, new List<int>(), new List<short>() { scale }, new List<SubEntity>());
        }

        public static List<int> GetDofusColors(List<int> colors)
        {
            int[] col = new int[colors.Count];
            for (int i = 0; i < colors.Count; i++)
            {
                var color = Color.FromArgb(colors.ToArray()[i]);
                col[i] = i + 1 << 24 | color.ToArgb() & 16777215;
            }
            return col.ToList();
        }
       
    }
}
