﻿#region License
/******************************************************************************
 * S7CommPlusDriver
 * 
 * Copyright (C) 2023 Thomas Wiens, th.wiens@gmx.de
 *
 * This file is part of S7CommPlusDriver.
 *
 * S7CommPlusDriver is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 /****************************************************************************/
#endregion

using System;
using System.IO;

namespace S7CommPlusDriver
{
    public class SetVariableRequest : IS7pSendableObject
    {
        public byte ProtocolVersion;
        public UInt16 SequenceNumber;
        public UInt32 SessionId;
        byte TransportFlags = 0x34;

        public UInt32 InObjectId;

        public UInt32 Address;
        public PValue Value;

        public bool WithIntegrityId = true;
        public UInt32 IntegrityId;

        public SetVariableRequest(byte protocolVersion)
        {
            ProtocolVersion = protocolVersion;
        }

        public byte GetProtocolVersion()
        {
            return ProtocolVersion;
        }

        public int Serialize(Stream buffer)
        {
            int ret = 0;
            ret += S7p.EncodeByte(buffer, Opcode.Request);
            ret += S7p.EncodeUInt16(buffer, 0);                               // Reserved
            ret += S7p.EncodeUInt16(buffer, Functioncode.SetVariable);
            ret += S7p.EncodeUInt16(buffer, 0);                               // Reserved
            ret += S7p.EncodeUInt16(buffer, SequenceNumber);
            ret += S7p.EncodeUInt32(buffer, SessionId);
            ret += S7p.EncodeByte(buffer, TransportFlags);

            // Request set
            ret += S7p.EncodeUInt32(buffer, InObjectId);
            ret += S7p.EncodeUInt32Vlq(buffer, 1); // Immer 1 (?)
            ret += S7p.EncodeUInt32Vlq(buffer, Address);
            ret += Value.Serialize(buffer);

            ret += S7p.EncodeObjectQualifier(buffer);
            // 1 Byte unbekannter Funktion
            ret += S7p.EncodeByte(buffer, 0x00);

            if (WithIntegrityId)
            {
                ret += S7p.EncodeUInt32Vlq(buffer, IntegrityId);
            }

            // Füllbytes?
            ret += S7p.EncodeUInt32(buffer, 0);

            return ret;
        }

        public override string ToString()
        {
            string s = "";
            s += "<SetVariableRequest>" + Environment.NewLine;
            s += "<ProtocolVersion>" + ProtocolVersion.ToString() + "</ProtocolVersion>" + Environment.NewLine;
            s += "<SequenceNumber>" + SequenceNumber.ToString() + "</SequenceNumber>" + Environment.NewLine;
            s += "<SessionId>" + SessionId.ToString() + "</SessionId>" + Environment.NewLine;
            s += "<TransportFlags>" + TransportFlags.ToString() + "</TransportFlags>" + Environment.NewLine;
            s += "<RequestSet>" + Environment.NewLine;
            s += "<InObjectId>" + InObjectId.ToString() + "</InObjectId>" + Environment.NewLine;
            s += "<AddressList>" + Environment.NewLine;
            s += "<Id>" + Address.ToString() + "</Id>" + Environment.NewLine;
            s += "</AddressList>" + Environment.NewLine;
            s += "<ValueList>" + Environment.NewLine;
            s += "<Value>" + Value.ToString() + "</Value>" + Environment.NewLine;
            s += "</ValueList>" + Environment.NewLine;
            s += "</RequestSet>" + Environment.NewLine;
            s += "</SetVariableRequest>" + Environment.NewLine;
            return s;
        }
    }
}
