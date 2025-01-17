﻿using Kudos.Constants;
using Kudos.Databasing.ORMs.GefyraModule.Builts;
using Kudos.Databasing.ORMs.GefyraModule.Constants;
using Kudos.Databasing.ORMs.GefyraModule.Descriptors;
using Kudos.Databasing.ORMs.GefyraModule.Entities;
using Kudos.Databasing.ORMs.GefyraModule.Enums;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Builders;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Clausoles;
using Kudos.Databasing.ORMs.GefyraModule.Interfaces.Entities;
using Kudos.Databasing.ORMs.GefyraModule.Types;

using Kudos.Databasing.ORMs.GefyraModule.Utils;
using Kudos.Utils.Collections;
using Kudos.Utils.Numerics;
using Kudos.Utils.Texts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kudos.Databasing.ORMs.GefyraModule.Builders
{
    internal class
        GefyraBuilder
    :
        IGefyraBuilder,
        IGefyraInsertClausoleBuilder,
            IGefyraIntoClausoleBuilder,
            IGefyraValuesClausoleBuilder,

        IGefyraSelectClausoleBuilder,
        IGefyraFromClausoleBuilder,
        IGefyraJoinClausoleBuilder,
        IGefyraOnClausoleBuilder,
            IGefyraOnActionClausoleBuilder,
            IGefyraCompareClausole<IGefyraOnActionClausoleBuilder>,
            IGefyraCompareClausoleBuilder<IGefyraOnActionClausoleBuilder>,
        IGefyraWhereClausoleBuilder,
            IGefyraWhereActionClausoleBuilder,
            IGefyraCompareClausoleBuilder<IGefyraWhereActionClausoleBuilder>,
            IGefyraCompareClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>,
            IGefyraCompareClausoleBuilder<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>,
            IGefyraMatchClausoleBuilder<IGefyraWhereActionClausoleBuilder>,
            IGefyraAgainstClausoleBuilder<IGefyraWhereActionClausoleBuilder>,

        IGefyraOrderByClausoleBuilder,
        IGefyraLimitClausoleBuilder,
        IGefyraOffsetClausoleBuilder,

        IGefyraCountClausoleBuilder,

        IGefyraUpdateClausoleBuilder,
            IGefyraUpdateClausole,
            IGefyraSetClausole,
            IGefyraSetClausoleBuilder,
            IGefyraSetActionClausoleBuilder,
            IGefyraPostClausole<IGefyraSetActionClausoleBuilder>,
            IGefyraPostClausoleBuilder<IGefyraSetActionClausoleBuilder>,

        IGefyraDeleteClausoleBuilder,
            IGefyraDeleteClausole
    {
        private static readonly String
            __sParameterPrefix;

        static GefyraBuilder()
        {
            // SAREBBE OPPORTUNO AGGIUNGERE UN FAKETIMESTAMP
            __sParameterPrefix =
                "GefyraParameter";
        }

        //private readonly Object _lgpck;
        private /*readonly*/ StringBuilder _sb0;
        private readonly StringBuilder _sb1;
        private /*readonly*/ List<GefyraParameter> _lgp;
        private /*readonly*/ List<IGefyraColumn> _lgcOnOutput, _lgcConsumed;
        private /*readonly*/ HashSet<IGefyraTable> _hsJoiningTables;

        internal GefyraBuilder()
        {
            //_lgpck = new object();
            _lgp = new List<GefyraParameter>();
            _lgcOnOutput = new List<IGefyraColumn>();
            _lgcConsumed = new List<IGefyraColumn>();
            _hsJoiningTables = new HashSet<IGefyraTable>();
            _sb0 = new StringBuilder();
            _sb1 = new StringBuilder();
        }

        #region private void _Append(...)

        private void _Append(ref EGefyraAgainst e)
        {
            String? s;
            GefyraAgainstUtils.GetString(ref e, out s);
            _Append(s);
        }

        private void _Append(ref EGefyraCompare e)
        {
            String? s;
            GefyraCompareUtils.GetString(ref e, out s);
            _Append(s);
        }

        private void _Append(ref EGefyraPost e)
        {
            String? s;
            GefyraPostUtils.GetString(ref e, out s);
            _Append(s);
        }

        private void _Append(ref EGefyraOrder e)
        {
            String? s;
            GefyraOrderUtils.GetString(ref e, out s);
            _Append(s);
        }

        private void _Append(ref EGefyraJoin e)
        {
            String? s;
            GefyraJoinUtils.GetString(ref e, out s);
            _Append(s);
        }

        private Boolean _Append(ref IGefyraTable? gt)
        {
            if (gt == null) return false;
            _Append(gt.GetSQL()); return true;
        }

        private void _Append(ref IGefyraColumn?[]? gca)
        {
            if (gca == null) return;
            for (int i = 0; i < gca.Length; i++)
            {
                _Append(ref gca[i]);
                if (i > gca.Length - 2) continue;
                _Append(CCharacter.Comma);
                _Append(CCharacter.Space);
            }
        }

        private void _Append(ref IGefyraColumn? gc)
        {
            if (gc == null) return;
            _Append(gc.GetSQL()); _lgcConsumed.Add(gc);
        }

        private void _Append(ref UInt32 i)
        {
            _sb0.Append(i);
        }

        private void _Append(ref Int32 i)
        {
            _sb0.Append(i);
        }

        private void _Append(String? s)
        {
            _sb0.Append(s);
        }

        private void _Remove(int i, int l)
        {
            _sb0.Remove(i, l);
        }

        private void _Append(Char? c)
        {
            _sb0.Append(c);
        }

        #endregion

        #region private void CalculateCurrentParameterMetas

        private void CalculateCurrentParameterMetas(out String s, out Int32 i)
        {
            i = _lgp.Count;
            _sb1.Clear();
            _sb1.Append(CCharacter.At).Append(__sParameterPrefix).Append(i);
            s = _sb1.ToString();
        }

        #endregion

        #region IGefyraInsertClausole

        public IGefyraInsertClausoleBuilder Insert()
        {
            _Append(CGefyraClausole.Insert);
            _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraIntoClausole

        public IGefyraIntoClausoleBuilder Into(IGefyraTable? gt, IGefyraColumn? gc, params IGefyraColumn?[]? gca)
        {
            _Append(CGefyraClausole.Into); _Append(CCharacter.Space); _Append(ref gt); _Append(CCharacter.Space);
            _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);

            IGefyraColumn?[]? gca0 = ArrayUtils.UnShift(gc, gca);

            _Append(ref gca0); _Append(CCharacter.Space);

            _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            //IGefyraColumn?[]? gca0 = ArrayUtils.UnShift(gc, gca);
            //_Append(ref gca0);
            //_Append(CCharacter.Space);

            return this;
        }

        #endregion

        #region IGefyraValuesClausole

        public IGefyraValuesClausoleBuilder Values(Object? o, params Object?[]? oa)
        {
            _Append(CGefyraClausole.Values); _Append(CCharacter.Space); _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);

            Object?[]? oa0 = ArrayUtils.UnShift(o, oa);

            if (oa0 != null)
            {
                IGefyraColumn? gci;
                for (int i = 0; i < oa0.Length; i++)
                {
                    gci = ListUtils.Get(_lgcConsumed, i);
                    if (gci == null) continue;
                    _AppendParameter(ref gci, ref oa0[i]);
                    if (i < oa0.Length - 1)
                        _Append(CCharacter.Comma);
                    _Append(CCharacter.Space);
                }
            }


            _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);

            return this;
        }

        #endregion

        #region IGefyraSelectClausole

        public IGefyraSelectClausoleBuilder Select(params IGefyraColumn?[]? gca)
        {
            _Append(CGefyraClausole.Select);
            _Append(CCharacter.Space);
            if (gca == null || gca.Length < 1 || ( gca.Length == 1 && gca[0] == null ) )
                _Append(CCharacter.Asterisk);
            else
            {
                _Append(ref gca);

                if (gca != null)
                    for (int i = 0; i < gca.Length; i++)
                    {
                        if (gca[i] == null) continue;
                        _lgcOnOutput.Add(gca[i]);
                    }
            }
            _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraCountClausole

        public IGefyraCountClausoleBuilder Count()
        {
            if (_lgcConsumed.Count < 1)
                _Remove(_sb0.Length - 2, 2);
            else
            {
                _Append(CCharacter.Comma);
                _Append(CCharacter.Space);
            }

            _Append(CGefyraClausole.Count);
            _Append(CCharacter.LeftRoundBracket);
            _Append(CCharacter.Asterisk);
            _Append(CCharacter.RightRoundBracket);
            _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraFromClausole

        public IGefyraFromClausoleBuilder From(IGefyraTable? gt)
        {
            _Append(CGefyraClausole.From); _Append(CCharacter.Space); _Append(ref gt); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraJoinClausole

        public IGefyraJoinClausoleBuilder LeftJoin(IGefyraTable? gt) { return _Join(EGefyraJoin.Left, ref gt); }
        public IGefyraJoinClausoleBuilder LeftJoin(Action<IGefyraSelectClausole>? act) { return _Join(EGefyraJoin.Left, ref act); }
        public IGefyraJoinClausoleBuilder RightJoin(IGefyraTable? gt) { return _Join(EGefyraJoin.Right, ref gt); }
        public IGefyraJoinClausoleBuilder RightJoin(Action<IGefyraSelectClausole>? act) { return _Join(EGefyraJoin.Right, ref act); }
        public IGefyraJoinClausoleBuilder InnerJoin(IGefyraTable? gt) { return _Join(EGefyraJoin.Inner, ref gt); }
        public IGefyraJoinClausoleBuilder InnerJoin(Action<IGefyraSelectClausole>? act) { return _Join(EGefyraJoin.Inner, ref act); }
        public IGefyraJoinClausoleBuilder FullJoin(IGefyraTable? gt) { return _Join(EGefyraJoin.Full, ref gt); }
        public IGefyraJoinClausoleBuilder FullJoin(Action<IGefyraSelectClausole>? act) { return _Join(EGefyraJoin.Full, ref act); }

        private IGefyraJoinClausoleBuilder _Join(EGefyraJoin e, ref IGefyraTable? gt)
        {
            _Append(ref e);
            _Append(CCharacter.Space);
            _Append(CGefyraClausole.Join);
            _Append(CCharacter.Space);
            if (_Append(ref gt)) _hsJoiningTables.Add(gt);
            _Append(CCharacter.Space);
            return this;
        }

        private IGefyraJoinClausoleBuilder _Join(EGefyraJoin e, ref Action<IGefyraSelectClausole>? actgsc)
        {
            _Append(ref e); _Append(CCharacter.Space); _Append(CGefyraClausole.Join); _Append(CCharacter.Space);
            _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);
            if (actgsc != null) actgsc.Invoke(this);
            _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraOnClausole

        public IGefyraOnClausoleBuilder On(Action<IGefyraOnActionClausoleBuilder>? act)
        {
            _Append(CGefyraClausole.On); _Append(CCharacter.Space);
            if (act != null) act.Invoke(this);
            return this;
        }

        #endregion

        #region IGefyraOnActionClausoleBuilder

        #region IGefyraCompareClausole

        IGefyraCompareClausoleBuilder<IGefyraOnActionClausoleBuilder> IGefyraCompareClausole<IGefyraOnActionClausoleBuilder>.Compare(IGefyraColumn? gc, EGefyraCompare e, object? o)
        {
            return _Compare(ref gc, ref e, ref o);
        }

        IGefyraCompareClausoleBuilder<IGefyraOnActionClausoleBuilder> IGefyraCompareClausole<IGefyraOnActionClausoleBuilder>.Compare(IGefyraColumn? gc, EGefyraCompare e, Action<IGefyraSelectClausole>? act)
        {
            return _Compare(ref gc, ref e, ref act);
        }

        #endregion

        #region IGefyraOpenBlockClausole

        IGefyraOnActionClausoleBuilder IGefyraOpenBlockClausole<IGefyraOnActionClausoleBuilder>.OpenBlock()
        {
            return _OpenBlock();
        }

        #endregion

        #region IGefyraAndClausole

        public IGefyraOnActionClausoleBuilder And()
        {
            return _And();
        }

        #endregion

        #region IGefyraOrClausole

        public IGefyraOnActionClausoleBuilder Or()
        {
            return _Or();
        }

        #endregion

        #endregion

        #region IGefyraCloseBlockClausole

        IGefyraJunctionClausole<IGefyraOnActionClausoleBuilder> IGefyraCloseBlockClausole<IGefyraJunctionClausole<IGefyraOnActionClausoleBuilder>>.CloseBlock()
        {
            return _CloseBlock();
        }

        #endregion

        #region IGefyraWhereClausole

        public IGefyraWhereClausoleBuilder Where(Action<IGefyraWhereActionClausoleBuilder>? act)
        {
            _Append(CGefyraClausole.Where); _Append(CCharacter.Space);
            if (act != null) act.Invoke(this);
            return this;
        }

        #endregion

        #region IGefyraWhereActionClausoleBuilder

        #region IGefyraCompareClausole

        public IGefyraCompareClausoleBuilder<IGefyraWhereActionClausoleBuilder> Compare(IGefyraColumn? gc, EGefyraCompare e, object? o)
        {
            return _Compare(ref gc, ref e, ref o);
        }

        public IGefyraCompareClausoleBuilder<IGefyraWhereActionClausoleBuilder> Compare(IGefyraColumn? gc, EGefyraCompare e, Action<IGefyraSelectClausole>? act)
        {
            return _Compare(ref gc, ref e, ref act);
        }

        IGefyraCompareClausoleBuilder<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>> IGefyraCompareClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>.Compare(IGefyraColumn? gc, EGefyraCompare e, object? o)
        {
            return _Compare(ref gc, ref e, ref o);
        }

        IGefyraCompareClausoleBuilder<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>> IGefyraCompareClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>.Compare(IGefyraColumn? gc, EGefyraCompare e, Action<IGefyraSelectClausole>? act)
        {
            return _Compare(ref gc, ref e, ref act);
        }

        #endregion

        #region IGefyraOpenBlockClausole

        public IGefyraWhereActionClausoleBuilder OpenBlock()
        {
            return _OpenBlock();
        }

        #endregion

        #region IGefyraCloseBlockClausole

        IGefyraJunctionClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>> IGefyraCloseBlockClausole<IGefyraJunctionClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>>.CloseBlock()
        {
            return _CloseBlock();
        }

        public IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder> CloseBlock()
        {
            return _CloseBlock();
        }

        #endregion

        #region IGefyraJunctionClausole

        IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder> IGefyraAndClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>.And()
        {
            return _And();
        }

        IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder> IGefyraOrClausole<IGefyraJunctionClausole<IGefyraWhereActionClausoleBuilder>>.Or()
        {
            return _Or();
        }

        #endregion

        #region IGefyraAndClausole

        IGefyraWhereActionClausoleBuilder IGefyraAndClausole<IGefyraWhereActionClausoleBuilder>.And()
        {
            return _And();
        }

        #endregion

        #region IGefyraOrClausole

        IGefyraWhereActionClausoleBuilder IGefyraOrClausole<IGefyraWhereActionClausoleBuilder>.Or()
        {
            return _Or();
        }

        #endregion

        #region IGefyraMatchClausole

        public IGefyraMatchClausoleBuilder<IGefyraWhereActionClausoleBuilder> Match(IGefyraColumn? gc, params IGefyraColumn[]? gca)
        {
            return _Match(ref gc, ref gca);
        }

        #endregion

        #region IGefyraAgainstClausole

        public IGefyraAgainstClausoleBuilder<IGefyraWhereActionClausoleBuilder> Against(string? s, EGefyraAgainst ega)
        {
            return _Against(ref s, ref ega);
        }

        #endregion

        #endregion

        #region IGefyraOrderByClausole

        public IGefyraOrderByClausoleBuilder OrderBy(IGefyraColumn? gc, EGefyraOrder e)
        {
            _Append(CGefyraClausole.Order); _Append(CCharacter.Space); _Append(CGefyraClausole.By); _Append(CCharacter.Space);
            _Append(ref gc); _Append(CCharacter.Space); _Append(ref e); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraLimitClausole

        public IGefyraLimitClausoleBuilder Limit(uint i)
        {
            _Append(CGefyraClausole.Limit); _Append(CCharacter.Space); _Append(ref i); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraOffsetClausole

        public IGefyraOffsetClausoleBuilder Offset(uint i)
        {
            _Append(CGefyraClausole.Offset); _Append(CCharacter.Space); _Append(ref i); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraUpdateClausole

        public IGefyraUpdateClausoleBuilder Update(IGefyraTable? gt)
        {
            _Append(CGefyraClausole.Update); _Append(CCharacter.Space); _Append(ref gt); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraDeleteClausole

        public IGefyraDeleteClausoleBuilder Delete()
        {
            _Append(CGefyraClausole.Delete); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraSetClausole

        public IGefyraSetClausoleBuilder Set(Action<IGefyraSetActionClausoleBuilder>? act)
        {
            _Append(CGefyraClausole.Set); _Append(CCharacter.Space);
            if (act != null) act.Invoke(this);
            return this;
        }

        #endregion

        #region IGefyraSetActionClausoleBuilder

        #region IGefyraPostClausole

        public IGefyraPostClausoleBuilder<IGefyraSetActionClausoleBuilder> Post(IGefyraColumn? gc, EGefyraPost egp, Object? o)
        {
            return _Post(ref gc, ref egp, ref o);
        }

        #endregion

        #region IGefyraAndClausole

        IGefyraSetActionClausoleBuilder IGefyraAndClausole<IGefyraSetActionClausoleBuilder>.And()
        {
            _Append(CCharacter.Comma); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #endregion

        #region IGefyraBuiltClausole

        public GefyraBuilt Build()
        {
            return new GefyraBuilt(ref _hsJoiningTables, ref _sb0, ref _lgp, ref _lgcOnOutput);
        }

        #endregion

        #region IGefyraCompareClausole

        private GefyraBuilder _Compare(ref IGefyraColumn? gc, ref EGefyraCompare e, ref object? o)
        {
            ICollection? c = o as ICollection;
            if (c == null)
            {
                _Append(ref gc); _Append(CCharacter.Space); _Append(ref e); _Append(CCharacter.Space);
                _AppendColumnOrParameter(ref gc, ref o); _Append(CCharacter.Space);
                return this;
            }

            Boolean bIsJointly = e == EGefyraCompare.In || e == EGefyraCompare.NotIn;

            if(bIsJointly)
            {
                _Append(ref gc); _Append(CCharacter.Space); _Append(ref e); _Append(CCharacter.Space);
            }

            _Append(CCharacter.LeftRoundBracket);

            IEnumerator eor = c.GetEnumerator();
            Object? oi;
            UInt16 i = 0;

            while (eor.MoveNext())
            {
                if (!bIsJointly)
                {
                    if (i > 0) { Or(); _Append(CCharacter.Space); }
                    _Append(ref gc); _Append(CCharacter.Space); _Append(ref e); _Append(CCharacter.Space);
                }
                else if (i > 0)
                {
                    _Append(CCharacter.Comma); _Append(CCharacter.Space);
                }

                oi = eor.Current;
                _AppendColumnOrParameter(ref gc, ref oi); _Append(CCharacter.Space);

                i++;
            }

            _Append(CCharacter.RightRoundBracket);
            
            return this;
        }


        private GefyraBuilder _Compare(ref IGefyraColumn? gc, ref EGefyraCompare e, ref Action<IGefyraSelectClausole>? act)
        {
            _Append(ref gc); _Append(CCharacter.Space); _Append(ref e); _Append(CCharacter.Space);
            _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);
            if (act != null) act.Invoke(this);
            _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraMatchClausole

        private GefyraBuilder _Match(ref IGefyraColumn? gc, ref IGefyraColumn[]? gca)
        {
            _Append(CGefyraClausole.Match); _Append(CCharacter.Space);
            IGefyraColumn?[]? gca0 = ArrayUtils.UnShift(gc, gca);
            _Append(CCharacter.LeftRoundBracket); _Append(ref gca0); _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraAgainstClausole

        private GefyraBuilder _Against(ref String? s, ref EGefyraAgainst ega)
        {
            _Append(CGefyraClausole.Against); _Append(CCharacter.Space);
            _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.SingleQuote); _Append(s); _Append(CCharacter.SingleQuote); _Append(CCharacter.Space);
            _Append(ref ega); _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraPostClausole

        private GefyraBuilder _Post(ref IGefyraColumn? gc, ref EGefyraPost e, ref object? o)
        {
            _Append(ref gc); _Append(CCharacter.Space); _Append(CCharacter.Equal); _Append(CCharacter.Space);

            if(e != EGefyraPost.Equal)
            {
                _Append(ref gc); _Append(CCharacter.Space);

                if (e == EGefyraPost.Addition)
                    _Append(CCharacter.Plus);
                else
                    _Append(CCharacter.Minus);

                _Append(CCharacter.Space);
            }

            _AppendColumnOrParameter(ref gc, ref o); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraOpenBlockClausole

        private GefyraBuilder _OpenBlock()
        {
            _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraCloseBlockClausole

        private GefyraBuilder _CloseBlock()
        {
            _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraJunctionClausole

        private GefyraBuilder _And()
        {
            _Append(CGefyraClausole.And); _Append(CCharacter.Space);
            return this;
        }

        private GefyraBuilder _Or()
        {
            _Append(CGefyraClausole.Or); _Append(CCharacter.Space);
            return this;
        }

        #endregion

        #region IGefyraExistsClausole

        public void Exists(Action<IGefyraSelectClausole> act)
        {
            _Append(CGefyraClausole.Exists);
            if (act != null) act.Invoke(this);
            _Append(CCharacter.Space);
        }

        #endregion

        //#region private void _AppendColumn(...)

        //private void _AppendColumn(ref IGefyraColumn? gc)
        //{
        //    _Append(ref gc); _Append(CCharacter.Space);
        //}

        //#endregion

        #region private void _AppendColumnOrParameter(...)

        private void _AppendColumnOrParameter(ref IGefyraColumn? gc, ref object? o)
        {
            IGefyraColumn? gc0 = o as IGefyraColumn;
            if (gc0 != null) _Append(ref gc0);
            else _AppendParameter(ref gc, ref o);
        }

        #endregion

        //private GefyraBuilder _CompareOrPost(ref IGefyraColumn? gc, ref EGefyraCompare? egc, ref EGefyraPost? egp, ref Action<IGefyraSelectClausole>? act)
        //{
        //    _Append(ref gc); _Append(CCharacter.Space); _Append(ref egc); _Append(ref egp); _Append(CCharacter.Space);
        //    _Append(CCharacter.LeftRoundBracket); _Append(CCharacter.Space);
        //    if (act != null) act.Invoke(this);
        //    _Append(CCharacter.RightRoundBracket); _Append(CCharacter.Space);
        //    return this;

        //}

        private void _AppendParameter(ref IGefyraColumn? gc, ref Object? o)
        {
            String s; Int32 i;
            CalculateCurrentParameterMetas(out s, out i);
            if (gc != null) _lgp.Add(new GefyraParameter(ref gc, ref i, ref s, ref o));
            _Append(s);
        }

        public override String ToString()
        {
            return _sb0.ToString();
        }
    }
}