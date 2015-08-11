﻿// A set of regression tests for equality/relational operators and the IComparer<> and IEqualityComparer<>
// implementation provided by ComparisonIdentity.Structural and HashIdentity.Structural

namespace FSharp.Core.Unittests.FSharp_Core.Microsoft_FSharp_Core

open System
open System.Numerics 
open FSharp.Core.Unittests.LibraryTestFx
open NUnit.Framework

module ComparersRegression =
    type RefWrap<'item> = { Item : 'item }

    [<Struct>]
    type ValueWrap<'item> =
        val Item : 'item
        new(item) = { Item = item }

    type UnionWrap<'item> =
    | UnionRaw of 'item
    | UnionRefWrap of RefWrap<'item>
    | UnionValueWrap of ValueWrap<'item>
    | UnionUnion of UnionWrap<'item>

    let createUnionWrap s =
        s
        |> Seq.collect (fun item ->
            [ UnionRaw item
              UnionRefWrap {Item = item}
              UnionValueWrap (ValueWrap item)
              UnionUnion (UnionRaw item)
              UnionUnion (UnionRefWrap {Item = item})
              UnionUnion (UnionValueWrap (ValueWrap item))
              UnionUnion (UnionUnion (UnionRaw item)) ])
        |> Array.ofSeq

    let createNullables s =
        seq {
            yield Nullable ()
            yield! s |> Seq.map (fun x -> Nullable x)
        }
        |> Array.ofSeq

    let createUnionTypes raw ref value union item =
        [| raw item
           ref item
           value item
           union (raw item)
           union (ref item)
           union (value item)
           union (union (raw item)) |]

    type Collection<'item, 'reftype, 'valuetype, 'uniontype> = {
        Array        : array<'item>
        ToRefType    : 'item -> 'reftype
        ToValueType  : 'item -> 'valuetype
        ToUnionTypes : 'item -> array<'uniontype>
    } with
        member this.ValueWrapArray =
            this.Array
            |> Array.map (fun item -> ValueWrap item)

        member this.RefWrapArray =
            this.Array
            |> Array.map (fun item -> { RefWrap.Item = item })

        member this.UnionWrapArray =
            this.Array
            |> createUnionWrap

        member this.ValueArray =
            this.Array
            |> Array.map this.ToValueType

        member this.RefArray =
            this.Array
            |> Array.map this.ToRefType

        member this.UnionArray =
            this.Array
            |> Array.collect this.ToUnionTypes

        member this.OptionArray =
            [|  yield None
                yield! this.Array |> Array.map Some |]

        member this.ArrayArray =
            [| yield! this.Array |> Array.map (fun x -> [| x |])
               yield! this.Array |> Array.mapi (fun i _ -> [| this.Array.[i]; this.Array.[(i+1)%this.Array.Length] |]) |]

        member this.ListArray =
            this.ArrayArray
            |> Array.map Array.toList

    module Bools =
        type TestType = bool

        let Values : array<TestType> = [| true; false|]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    module NullableBools =
        type TestType = Nullable<bool>

        let Values : array<TestType> = createNullables Bools.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module SBytes =
        type TestType = sbyte

        let Values : array<TestType> = [| SByte.MinValue; SByte.MaxValue; -1y; 0y; +1y |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableSbytes =
        type TestType = Nullable<sbyte>

        let Values : array<TestType> = createNullables SBytes.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Int16s =
        type TestType = int16

        let Values : array<TestType> = [| Int16.MaxValue; Int16.MaxValue; -1s; 0s; +1s |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableInt16s =
        type TestType = Nullable<int16>

        let Values : array<TestType> = createNullables Int16s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Int32s =
        type TestType = int32

        let Values : array<TestType> = [| Int32.MinValue; Int32.MaxValue; -1; 0; +1 |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableInt32s =
        type TestType = Nullable<int32>

        let Values : array<TestType> = createNullables Int32s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Int64s =
        type TestType = int64

        let Values : array<TestType> = [| Int64.MinValue; Int64.MaxValue; -1L; 0L; +1L |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableInt64s =
        type TestType = Nullable<int64>

        let Values : array<TestType> = createNullables Int64s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NativeInts =
        type TestType = nativeint

        let Values : array<TestType> = [| -1n; 0n; +1n |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableNativeInts =
        type TestType = Nullable<nativeint>

        let Values : array<TestType> = createNullables NativeInts.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Bytes =
        type TestType = byte

        let Values : array<TestType> = [| Byte.MinValue; Byte.MaxValue; 0uy; 1uy; 2uy |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableBytes =
        type TestType = Nullable<byte>

        let Values : array<TestType> = createNullables Bytes.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Uint16s =
        type TestType = uint16

        let Values : array<TestType> = [| UInt16.MinValue; UInt16.MaxValue; 0us; 1us; 2us |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableUInt16s =
        type TestType = Nullable<uint16>

        let Values : array<TestType> = createNullables Uint16s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module UInt32s =
        type TestType = uint32

        let Values : array<TestType> = [| UInt32.MinValue; UInt32.MaxValue; 0u; 1u; 2u|]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableUInt32s =
        type TestType = Nullable<uint32>

        let Values : array<TestType> = createNullables UInt32s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module UInt64s =
        type TestType = uint64

        let Values : array<TestType> = [| UInt64.MinValue; UInt64.MaxValue; 0UL; 1UL; 2UL|]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableUInt64s =
        type TestType = Nullable<uint64>

        let Values : array<TestType> = createNullables UInt64s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module UNativeInts =
        type TestType = unativeint

        let Values : array<TestType> = [| 0un; 1un; 2un |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableUNativeInts =
        type TestType = Nullable<unativeint>

        let Values : array<TestType> = createNullables UNativeInts.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Chars =
        type TestType = char

        let Values : array<TestType> = [| Char.MinValue; Char.MaxValue; '0'; '1'; '2' |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableChars =
        type TestType = Nullable<char>

        let Values : array<TestType> = createNullables Chars.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Strings =
        type TestType = string

        let Values : array<TestType> = [| null; String.Empty; "Hello, world!"; String('\u0000', 3); "\u0061\u030a"; "\u00e5" |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Decimals =
        type TestType = decimal

        let Values : array<TestType> = [| Decimal.MinValue; Decimal.MaxValue; Decimal.MinusOne; Decimal.Zero; Decimal.One |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableDecimals =
        type TestType = Nullable<decimal>

        let Values : array<TestType> = createNullables Decimals.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Floats =
        type TestType = float

        let Values : array<TestType> = [| Double.MinValue; Double.MaxValue; Double.Epsilon; Double.NaN; Double.NegativeInfinity; Double.PositiveInfinity; -1.; 0.; 1. |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableFloats =
        type TestType = Nullable<float>

        let Values : array<TestType> = createNullables Floats.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Float32s =
        type TestType = float32

        let Values : array<TestType> = [| Single.MinValue; Single.MaxValue; Single.Epsilon; Single.NaN; Single.NegativeInfinity; Single.PositiveInfinity; -1.f; 0.f; 1.f |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableFloat32s =
        type TestType = Nullable<float32>

        let Values : array<TestType> = createNullables Float32s.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module DateTimes =
        type TestType = System.DateTime

        let Values : array<TestType> = [| DateTime.MinValue; DateTime.MaxValue; DateTime(2015, 10, 8, 5, 39, 23) |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module NullableDateTimes =
        type TestType = Nullable<DateTime>

        let Values : array<TestType> = createNullables DateTimes.Values
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Tuple2s =
        type TestType = float*float

        let Values : array<TestType> = [| (nan, nan); (nan, 0.0); (0.0, nan); (0.0, 0.0) |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }
    // ----------------------------------------------------------------------------

    module Tuple3s =
        type TestType = float*float*float

        let Values : array<TestType> = [|
            (nan, nan, nan); (nan, nan, 0.0); (nan, 0.0, nan); (nan, 0.0, 0.0);
            (0.0, nan, nan); (0.0, nan, 0.0); (0.0, 0.0, nan); (0.0, 0.0, 0.0) |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Tuple4s =
        type TestType = float*float*float*float

        let Values : array<TestType> = [|
            (nan, nan, nan, nan); (nan, nan, nan, 0.0); (nan, nan, 0.0, nan); (nan, nan, 0.0, 0.0);
            (nan, 0.0, nan, nan); (nan, 0.0, nan, 0.0); (nan, 0.0, 0.0, nan); (nan, 0.0, 0.0, 0.0);
            (0.0, nan, nan, nan); (0.0, nan, nan, 0.0); (0.0, nan, 0.0, nan); (0.0, nan, 0.0, 0.0);
            (0.0, 0.0, nan, nan); (0.0, 0.0, nan, 0.0); (0.0, 0.0, 0.0, nan); (0.0, 0.0, 0.0, 0.0);
        |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    module Tuple5s =
        type TestType = float*float*float*float*float

        let Values : array<TestType> = [|
            (nan, nan, nan, nan, nan); (nan, nan, nan, nan, 0.0); (nan, nan, nan, 0.0, nan); (nan, nan, nan, 0.0, 0.0);
            (nan, nan, 0.0, nan, nan); (nan, nan, 0.0, nan, 0.0); (nan, nan, 0.0, 0.0, nan); (nan, nan, 0.0, 0.0, 0.0);
            (nan, 0.0, nan, nan, nan); (nan, 0.0, nan, nan, 0.0); (nan, 0.0, nan, 0.0, nan); (nan, 0.0, nan, 0.0, 0.0);
            (nan, 0.0, 0.0, nan, nan); (nan, 0.0, 0.0, nan, 0.0); (nan, 0.0, 0.0, 0.0, nan); (nan, 0.0, 0.0, 0.0, 0.0);
            (0.0, nan, nan, nan, nan); (0.0, nan, nan, nan, 0.0); (0.0, nan, nan, 0.0, nan); (0.0, nan, nan, 0.0, 0.0);
            (0.0, nan, 0.0, nan, nan); (0.0, nan, 0.0, nan, 0.0); (0.0, nan, 0.0, 0.0, nan); (0.0, nan, 0.0, 0.0, 0.0);
            (0.0, 0.0, nan, nan, nan); (0.0, 0.0, nan, nan, 0.0); (0.0, 0.0, nan, 0.0, nan); (0.0, 0.0, nan, 0.0, 0.0);
            (0.0, 0.0, 0.0, nan, nan); (0.0, 0.0, 0.0, nan, 0.0); (0.0, 0.0, 0.0, 0.0, nan); (0.0, 0.0, 0.0, 0.0, 0.0);
        |]
    
        type RefType = {
            Item : TestType
        }
    
        [<Struct>]
        type ValueType =
            val Item : TestType
            new(item) = { Item = item }

        type UnionType =
        | UnionRaw of TestType
        | UnionRefType of RefType
        | UnionValueType of ValueType
        | UnionUnion of UnionType

        let toRefType = fun x -> { Item = x}
        let toValueType = fun x -> ValueType x

        let createUnion =
            let raw x = UnionRaw x
            let ref x = UnionRefType (toRefType x)
            let value x = UnionValueType (toValueType x)
            let union x = UnionUnion x
            createUnionTypes raw ref value union

        let Collection = {
            Array        = Values
            ToRefType    = fun x -> { Item = x}
            ToValueType  = fun x -> ValueType x
            ToUnionTypes = createUnion
        }

    // ----------------------------------------------------------------------------

    exception ValidationException of lhs:obj * rhs:obj * expected:obj * received:obj

    let make_result_set<'a,'b when 'b : equality> (f:'a->'a->'b) (items:array<'a>) (validation_set:option<array<'b>>)=
        let results = Array.zeroCreate (items.Length*items.Length)
        for i = 0 to items.Length-1 do
            for j = 0 to items.Length-1 do
                let index = i * items.Length + j
                let lhs = items.[i] 
                let rhs = items.[j]
                let result = f lhs rhs

                validation_set
                |> Option.iter (fun validation_set ->
                    let expected = validation_set.[index]
                    if expected <> result then
                        raise (ValidationException (box lhs, box rhs, expected, result)))

                results.[index] <- result
        results

#if FX_ATLEAST_45
    let create<'a,'b when 'b : equality> name operation (f:'a->'a->'b) (items:array<'a>) =
        printf """ [<Test>]
  member __.``%s %s``() =
   validate (%s) %s """ name operation name operation

        make_result_set f items None
        |> Seq.iteri (fun n result ->
            if n = 0 
                then printf "[|"
                else printf "; "
            if n % 20 = 0 then printf "\n    "
            printf "%A" result)
        printfn "\n   |]\n"

    let create_inequalities name (items:array<'a>) =
        create name "(fun x y -> ComparisonIdentity.Structural.Compare(x,y))" (fun x y -> ComparisonIdentity.Structural.Compare(x,y)) items
        create name "(fun x y -> HashIdentity.Structural.Equals(x,y))" (fun x y -> HashIdentity.Structural.Equals(x,y)) items
        create name "(>)"  (>)  items   
        create name "(>=)" (>=) items  
        create name "(<=)" (<=) items  
        create name "(<)"  (<)  items   
        create name "(=)"  (=)  items   
        create name "(<>)" (<>) items

    let create_equalities name (items:array<'a>) =
        create name "(fun x y -> HashIdentity.Structural.Equals(x,y))" (fun x y -> HashIdentity.Structural.Equals(x,y)) items
        create name "(=)"  (=) items
        create name "(<>)" (<>) items

    let create_collection_inequalities name (collection:Collection<_,_,_,_>) =
        create_inequalities (name + ".Array")          collection.Array
        create_inequalities (name + ".OptionArray")    collection.OptionArray
        create_inequalities (name + ".RefArray")       collection.RefArray
        create_inequalities (name + ".RefWrapArray")   collection.RefWrapArray
        create_inequalities (name + ".UnionArray")     collection.UnionArray
        create_inequalities (name + ".UnionWrapArray") collection.UnionWrapArray
        create_inequalities (name + ".ValueArray")     collection.ValueArray
        create_inequalities (name + ".ValueWrapArray") collection.ValueWrapArray
        create_inequalities (name + ".ArrayArray")     collection.ArrayArray
        create_inequalities (name + ".ListArray")      collection.ListArray
        create_inequalities (name + ".ArrayArray |> Array.map Set.ofArray") (collection.ArrayArray |> Array.map Set.ofArray)

    let create_tuples_tests name (collection:Collection<_,_,_,_>) =
        create_inequalities (name + ".Array")          collection.Array

    let create_collection_equalities name (collection:Collection<_,_,_,_>) =
        create_equalities (name + ".Array")          collection.Array
        create_equalities (name + ".OptionArray")    collection.OptionArray
        create_equalities (name + ".RefArray")       collection.RefArray
        create_equalities (name + ".RefWrapArray")   collection.RefWrapArray
        create_equalities (name + ".UnionArray")     collection.UnionArray
        create_equalities (name + ".UnionWrapArray") collection.UnionWrapArray
        create_equalities (name + ".ValueArray")     collection.ValueArray
        create_equalities (name + ".ValueWrapArray") collection.ValueWrapArray
        create_equalities (name + ".ArrayArray")     collection.ArrayArray
        create_equalities (name + ".ListArray")      collection.ListArray

    let createData () =
        create_collection_inequalities "Bools.Collection"               Bools.Collection
        create_collection_equalities   "NullableBools.Collection"       NullableBools.Collection
        create_collection_inequalities "SBytes.Collection"              SBytes.Collection
        create_collection_equalities   "NullableSbytes.Collection"      NullableSbytes.Collection
        create_collection_inequalities "Int16s.Collection"              Int16s.Collection
        create_collection_equalities   "NullableInt16s.Collection"      NullableInt16s.Collection
        create_collection_inequalities "Int32s.Collection"              Int32s.Collection
        create_collection_equalities   "NullableInt32s.Collection"      NullableInt32s.Collection
        create_collection_inequalities "Int64s.Collection"              Int64s.Collection
        create_collection_equalities   "NullableInt64s.Collection"      NullableInt64s.Collection
        create_collection_inequalities "NativeInts.Collection"          NativeInts.Collection
        create_collection_equalities   "NullableNativeInts.Collection"  NullableNativeInts.Collection
        create_collection_inequalities "Bytes.Collection"               Bytes.Collection
        create_collection_equalities   "NullableBytes.Collection"       NullableBytes.Collection
        create_collection_inequalities "Uint16s.Collection"             Uint16s.Collection
        create_collection_equalities   "NullableUInt16s.Collection"     NullableUInt16s.Collection
        create_collection_inequalities "UInt32s.Collection"             UInt32s.Collection
        create_collection_equalities   "NullableUInt32s.Collection"     NullableUInt32s.Collection
        create_collection_inequalities "UInt64s.Collection"             UInt64s.Collection
        create_collection_equalities   "NullableUInt64s.Collection"     NullableUInt64s.Collection
        create_collection_inequalities "UNativeInts.Collection"         UNativeInts.Collection
        create_collection_equalities   "NullableUNativeInts.Collection" NullableUNativeInts.Collection
        create_collection_inequalities "Chars.Collection"               Chars.Collection
        create_collection_equalities   "NullableChars.Collection"       NullableChars.Collection
        create_collection_inequalities "Strings.Collection"             Strings.Collection
        create_collection_inequalities "Decimals.Collection"            Decimals.Collection
        create_collection_equalities   "NullableDecimals.Collection"    NullableDecimals.Collection
        create_collection_inequalities "Floats.Collection"              Floats.Collection
        create_collection_equalities   "NullableFloats.Collection"      NullableFloats.Collection
        create_collection_inequalities "Float32s.Collection"            Float32s.Collection
        create_collection_equalities   "NullableFloat32s.Collection"    NullableFloat32s.Collection
        create_collection_inequalities "DateTimes.Collection"           DateTimes.Collection
        create_collection_equalities   "NullableDateTimes.Collection"   NullableDateTimes.Collection
        create_collection_inequalities "Tuple2s.Collection"             Tuple2s.Collection
        create_tuples_tests            "Tuple3s.Collection"             Tuple3s.Collection
        create_tuples_tests            "Tuple4s.Collection"             Tuple4s.Collection
        create_tuples_tests            "Tuple5s.Collection"             Tuple5s.Collection
#endif

    let validate (items:array<'a>) (f:'a->'a->'b) (expected:array<'b>) =
        try
            make_result_set f items (Some expected) |> ignore
        with
        | ValidationException(lhs=lhs; rhs=rhs; expected=expected; received=received) ->
            failwith <| sprintf "args(%O, %O) Expected=%O. Received=%O." lhs rhs expected received

open ComparersRegression

[<TestFixture>]
type GeneratedTestSuite () =
 let _ = ()
// ------------------------------------------------------------------------------
// -- The following should be generated by running CreateComparersRegression.fsx
// ------------------------------------------------------------------------------
 