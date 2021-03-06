﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschränkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Newtonsoft.Json;
using Squidex.Infrastructure.Json;
using Squidex.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Squidex.Domain.Apps.Core.Apps.Json
{
    public sealed class RolesConverter : JsonClassConverter<Roles>
    {
        protected override void WriteValue(JsonWriter writer, Roles value, JsonSerializer serializer)
        {
            var json = new Dictionary<string, string[]>(value.Count);

            foreach (var role in value)
            {
                json.Add(role.Key, role.Value.Permissions.ToIds().ToArray());
            }

            serializer.Serialize(writer, json);
        }

        protected override Roles ReadValue(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var json = serializer.Deserialize<Dictionary<string, string[]>>(reader);

            return new Roles(json.ToImmutableDictionary(x => x.Key, x => new Role(x.Key, new PermissionSet(x.Value))));
        }
    }
}
