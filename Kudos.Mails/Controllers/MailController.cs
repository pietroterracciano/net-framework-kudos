﻿using Kudos.Mails.Models;
using Kudos.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Kudos.Mails.Controllers
{ 
    public class MailController 
    {
        private MConfigModel 
            _mConfig;

        public MConfigModel Config
        {
            get;
            private set;
        }

        public MailController()
        {
            _mConfig = new MConfigModel();
        }

        #region public Boolean[] Send()

        /// <summary>Nullable</summary>
        public Boolean[] Send(MMessageModel mMessage, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            Task<Boolean[]> oTask = SendAsync(mMessage, i32SendNextAfterNMilliSeconds);
            oTask.Wait();
            return oTask.Result;
        }

        /// <summary>Nullable</summary>
        public Boolean[] Send(List<MMessageModel> lMessages, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            Task<Boolean[]> oTask = SendAsync(lMessages, i32SendNextAfterNMilliSeconds);
            if (oTask == null) return null;
            oTask.Wait();
            return oTask.Result;
        }

        /// <summary>Nullable</summary>
        public Boolean[] Send(MMessageModel[] aMessages, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            Task<Boolean[]> oTask = SendAsync(aMessages, i32SendNextAfterNMilliSeconds);
            oTask.Wait();
            return oTask.Result;
        }

        #endregion

        #region  public Task<Boolean[]> SendAsync()

        public Task<Boolean[]> SendAsync(MMessageModel mMessage, SendCompletedEventHandler oOnCompleted = null, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            return SendAsync(new MMessageModel[] { mMessage }, i32SendNextAfterNMilliSeconds, oOnCompleted);
        }
        public Task<Boolean[]> SendAsync(MMessageModel mMessage, Int32 i32SendNextAfterNMilliSeconds = 1000, SendCompletedEventHandler oOnCompleted = null)
        {
            return SendAsync(new MMessageModel[] { mMessage }, i32SendNextAfterNMilliSeconds, oOnCompleted);
        }

        /// <summary>Nullable</summary>
        public Task<Boolean[]> SendAsync(List<MMessageModel> lMessages, SendCompletedEventHandler oOnCompleted = null, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            return lMessages != null ? SendAsync(lMessages.ToArray(), i32SendNextAfterNMilliSeconds, oOnCompleted) : null;
        }
        /// <summary>Nullable</summary>
        public Task<Boolean[]> SendAsync(List<MMessageModel> lMessages, Int32 i32SendNextAfterNMilliSeconds = 1000, SendCompletedEventHandler oOnCompleted = null)
        {
            return lMessages != null ? SendAsync(lMessages.ToArray(), i32SendNextAfterNMilliSeconds, oOnCompleted) : null;
        }

        public Task<Boolean[]> SendAsync(MMessageModel[] aMessages, SendCompletedEventHandler oOnCompleted = null, Int32 i32SendNextAfterNMilliSeconds = 1000)
        {
            return SendAsync(aMessages, i32SendNextAfterNMilliSeconds, oOnCompleted);
        }
        public Task<Boolean[]> SendAsync(MMessageModel[] aMessages, Int32 i32SendNextAfterNMilliSeconds = 1000, SendCompletedEventHandler oOnCompleted = null)
        {
            return Task.Run<Boolean[]>(
                () =>
                {
                    if (aMessages == null)
                        return null;

                    SmtpClient
                        oSmtpClient = _mConfig.ToSmtpClient();

                    if (oSmtpClient == null)
                        return null;

                    if (oOnCompleted != null)
                        oSmtpClient.SendCompleted += oOnCompleted;

                    Boolean[]
                        aResults = new Boolean[aMessages.Length];

                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(_RemoteCertificateValidationCallback);

                    for (Int32 i = 0; i < aMessages.Length; i++)
                    {
                        if (aMessages[i] == null)
                        {
                            aResults[i] = false;
                            continue;
                        }

                        MailMessage
                            oMailMessage = aMessages[i].ToMailMessage();

                        if (oMailMessage == null)
                        {
                            aResults[i] = false;
                            continue;
                        }

                        try
                        {
                            oSmtpClient.Send(oMailMessage);
                            aResults[i] = true;
                        }
                        catch
                        {
                            aResults[i] = false;
                        }

                        aMessages[i].FlushAttachments();
                        try { oMailMessage.Attachments.Dispose(); } catch { }

                        if (i < (aMessages.Length - 1))
                            Thread.Sleep(i32SendNextAfterNMilliSeconds);
                    }

                    return aResults;
                }
            );

        }

        #endregion

        private static bool _RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}