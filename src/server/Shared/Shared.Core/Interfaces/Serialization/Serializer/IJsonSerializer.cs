﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IJsonSerializer.cs" company="HureIT">
// Copyright (c) HureIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace HureIT.Shared.Core.Interfaces.Serialization.Serializer
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj, IJsonSerializerSettingsOptions settings = null);

        string Serialize<T>(T obj, Type type, IJsonSerializerSettingsOptions settings = null);

        T Deserialize<T>(string text, IJsonSerializerSettingsOptions settings = null);
    }
}