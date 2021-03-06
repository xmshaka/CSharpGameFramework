﻿using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class GetUserInfoValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getuserinfo" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetUserInfoValue val = new GetUserInfoValue();
            val.m_UserGuid = m_UserGuid.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_UserGuid.Evaluate(instance, iterator, args);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                if (m_UserGuid.HaveValue) {
                    ulong userGuid = m_UserGuid.Value;
                    m_HaveValue = true;
                    m_Value = UserServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
                }
            }
        }
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
