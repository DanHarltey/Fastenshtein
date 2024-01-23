## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00E7240]
       test      r10d,r10d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00E7260]
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 130
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00D72E0]
       lea       r8,[rax+10]
       test      r10d,r10d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00D7300]
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 129
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00F7280]
       lea       r10,[rax+10]
       test      r8d,r8d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00F72A0]
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 123
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D72A0]
       lea       r10,[rax+10]
       test      r8d,r8d
       jg        short M00_L03
M00_L00:
       test      ecx,ecx
       jl        short M00_L02
       cmp       edx,ecx
       jle       short M00_L02
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
M00_L02:
       vzeroupper
       ret
M00_L03:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00D72C0]
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L03
       jmp       short M00_L00
; Total bytes of code 89
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C7260]
       test      r10d,r10d
       jg        short M00_L03
M00_L00:
       cmp       r8d,edx
       jle       short M00_L02
M00_L01:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       cmp       r8d,edx
       jg        short M00_L01
M00_L02:
       vzeroupper
       ret
M00_L03:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00C7280]
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L03
       jmp       short M00_L00
; Total bytes of code 87
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00F7240]
       test      r10d,r10d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00F7260]
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 130
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00E72E0]
       lea       r8,[rax+10]
       test      r10d,r10d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
       nop       word ptr [rax+rax]
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00E7300]
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 139
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D7280]
       lea       r10,[rax+10]
       test      r8d,r8d
       jg        short M00_L02
M00_L00:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
       jmp       short M00_L04
M00_L02:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00D72A0]
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L02
       jmp       short M00_L00
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 123
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D72A0]
       lea       r10,[rax+10]
       test      r8d,r8d
       jg        short M00_L03
M00_L00:
       test      ecx,ecx
       jl        short M00_L02
       cmp       edx,ecx
       jle       short M00_L02
M00_L01:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L01
M00_L02:
       vzeroupper
       ret
M00_L03:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00D72C0]
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L03
       jmp       short M00_L00
; Total bytes of code 89
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C7260]
       test      r10d,r10d
       jg        short M00_L03
M00_L00:
       cmp       r8d,edx
       jle       short M00_L02
M00_L01:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       cmp       r8d,edx
       jg        short M00_L01
M00_L02:
       vzeroupper
       ret
M00_L03:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,[7FFCF00C7280]
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L03
       jmp       short M00_L00
; Total bytes of code 87
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00D7240]
       test      r10d,r10d
       jle       short M00_L04
       cmp       edx,r10d
       jl        short M00_L01
       vmovups   ymm1,[7FFCF00D7260]
M00_L00:
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vmovups   ymm1,[7FFCF00D7260]
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L01
M00_L02:
       cmp       edx,ecx
       jg        short M00_L05
M00_L03:
       vzeroupper
       add       rsp,28
       ret
M00_L04:
       cmp       edx,ecx
       jle       short M00_L03
M00_L05:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L04
M00_L06:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 151
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00C72C0]
       lea       r8,[rax+10]
       test      r10d,r10d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00C72E0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       cmp       edx,ecx
       jg        short M00_L03
M00_L01:
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       cmp       edx,ecx
       jle       short M00_L01
M00_L03:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L02
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 113
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00F7280]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00F72A0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
       cmp       edx,ecx
       jg        short M00_L03
M00_L01:
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       cmp       edx,ecx
       jle       short M00_L01
M00_L03:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L02
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 107
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00F72C0]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00F72E0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       test      ecx,ecx
       jl        short M00_L02
       cmp       edx,ecx
       jg        short M00_L03
M00_L02:
       vzeroupper
       add       rsp,28
       ret
M00_L03:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
       jmp       short M00_L02
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 111
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00E7260]
       test      r10d,r10d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00E7280]
M00_L00:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L00
       cmp       r8d,edx
       jg        short M00_L03
M00_L01:
       vzeroupper
       ret
M00_L02:
       cmp       r8d,edx
       jle       short M00_L01
M00_L03:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       jmp       short M00_L02
; Total bytes of code 91
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00E7260]
       test      r10d,r10d
       jle       short M00_L02
       cmp       edx,r10d
       jl        short M00_L01
       vmovups   ymm1,[7FFCF00E7280]
M00_L00:
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       jmp       short M00_L02
       nop       dword ptr [rax]
M00_L01:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vmovups   ymm1,[7FFCF00E7280]
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L01
M00_L02:
       cmp       edx,ecx
       jle       short M00_L05
       test      ecx,ecx
       jl        short M00_L04
M00_L03:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
       jmp       short M00_L05
M00_L04:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L04
M00_L05:
       vzeroupper
       add       rsp,28
       ret
M00_L06:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 176
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00C72E0]
       lea       r8,[rax+10]
       test      r10d,r10d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00C7300]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
M00_L01:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
       nop       dword ptr [rax+rax]
       nop       dword ptr [rax+rax]
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
       jmp       short M00_L04
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 144
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D7280]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00D72A0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
       jmp       short M00_L04
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 125
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D72A0]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00D72C0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       test      ecx,ecx
       jl        short M00_L03
       cmp       edx,ecx
       jle       short M00_L03
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
M00_L03:
       vzeroupper
       ret
; Total bytes of code 91
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C7260]
       test      r10d,r10d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00C7280]
M00_L00:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L00
M00_L01:
       cmp       r8d,edx
       jle       short M00_L03
M00_L02:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       cmp       r8d,edx
       jg        short M00_L02
M00_L03:
       vzeroupper
       ret
; Total bytes of code 89
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00D7440]
       test      r10d,r10d
       jle       short M00_L04
       cmp       edx,r10d
       jl        short M00_L01
       vmovups   ymm1,[7FFCF00D7460]
       nop       word ptr [rax+rax]
M00_L00:
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vmovups   ymm1,[7FFCF00D7460]
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L01
M00_L02:
       cmp       edx,ecx
       jg        short M00_L05
M00_L03:
       vzeroupper
       add       rsp,28
       ret
M00_L04:
       cmp       edx,ecx
       jle       short M00_L03
M00_L05:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L04
M00_L06:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 160
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00C7480]
       lea       r8,[rax+10]
       test      r10d,r10d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00C74A0]
       nop       word ptr [rax+rax]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       cmp       edx,ecx
       jg        short M00_L03
M00_L01:
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       cmp       edx,ecx
       jle       short M00_L01
M00_L03:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L02
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 123
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00F7440]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00F7460]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
       cmp       edx,ecx
       jg        short M00_L03
M00_L01:
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       cmp       edx,ecx
       jle       short M00_L01
M00_L03:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       jmp       short M00_L02
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 107
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00F7480]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00F74A0]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       test      ecx,ecx
       jl        short M00_L03
       cmp       edx,ecx
       jle       short M00_L03
M00_L02:
       cmp       ecx,edx
       jae       short M00_L04
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
M00_L03:
       vzeroupper
       add       rsp,28
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 109
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00D73A0]
       test      r10d,r10d
       jle       short M00_L02
       vmovups   ymm1,[7FFCF00D73C0]
       nop       dword ptr [rax]
M00_L00:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L00
       cmp       r8d,edx
       jg        short M00_L03
M00_L01:
       vzeroupper
       ret
M00_L02:
       cmp       r8d,edx
       jle       short M00_L01
M00_L03:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       jmp       short M00_L02
; Total bytes of code 94
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref tmp[i]);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00D7480]
       test      r10d,r10d
       jle       short M00_L02
       cmp       edx,r10d
       jl        short M00_L01
       vmovups   ymm1,[7FFCF00D74A0]
       nop       word ptr [rax+rax]
M00_L00:
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       vmovups   [rax+r8*4+10],ymm0
       vmovups   ymm1,[7FFCF00D74A0]
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L01
M00_L02:
       cmp       edx,ecx
       jle       short M00_L05
       test      ecx,ecx
       jl        short M00_L04
M00_L03:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
       jmp       short M00_L05
M00_L04:
       cmp       ecx,edx
       jae       short M00_L06
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L04
M00_L05:
       vzeroupper
       add       rsp,28
       ret
M00_L06:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 178
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate2()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,7
       mov       r10d,edx
       sub       r10d,r8d
       vmovups   ymm0,[7FFCF00E7480]
       lea       r8,[rax+10]
       test      r10d,r10d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00E74A0]
       nop       word ptr [rax+rax]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r8+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r10d
       jl        short M00_L00
M00_L01:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
       nop       dword ptr [rax]
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
       jmp       short M00_L04
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 144
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate3()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C7420]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00C7440]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       cmp       edx,ecx
       jle       short M00_L04
       test      ecx,ecx
       jl        short M00_L03
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
       jmp       short M00_L04
M00_L03:
       cmp       ecx,edx
       jae       short M00_L05
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L03
M00_L04:
       vzeroupper
       add       rsp,28
       ret
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 125
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate4()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;             if (i >= 0)
;             ^^^^^^^^^^^
;                     tmp[i] = i;
;                     ^^^^^^^^^^^
;                 for (; i < tmp.Length; i++)
;                                        ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       mov       r8d,edx
       and       r8d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C7460]
       lea       r10,[rax+10]
       test      r8d,r8d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00C7480]
       nop       dword ptr [rax]
M00_L00:
       movsxd    r9,ecx
       vmovups   [r10+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       ecx,8
       cmp       ecx,r8d
       jl        short M00_L00
M00_L01:
       test      ecx,ecx
       jl        short M00_L03
       cmp       edx,ecx
       jle       short M00_L03
M00_L02:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L02
M00_L03:
       vzeroupper
       ret
; Total bytes of code 95
```

## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.VectorPopulate5()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
;             ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 var lastBlockIndex = tmp.Length & vectorMask;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 var previous = indexesVector;
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous.StoreUnsafe(ref itemRef);
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     previous += additionVector;
;                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                     i += Vector<int>.Count;
;                     ^^^^^^^^^^^^^^^^^^^^^^^
;                 while (i < lastBlockIndex)
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
;                 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
;                 itemRef = i;
;                 ^^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       vzeroupper
       mov       rax,[rcx+8]
       lea       rcx,[rax+10]
       xor       edx,edx
       mov       r8d,[rax+8]
       mov       r10d,r8d
       and       r10d,0FFFFFFF8
       vmovups   ymm0,[7FFCF00C73A0]
       test      r10d,r10d
       jle       short M00_L01
       vmovups   ymm1,[7FFCF00C73C0]
       nop       dword ptr [rax]
M00_L00:
       movsxd    r9,edx
       vmovups   [rcx+r9*4],ymm0
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       edx,r10d
       jl        short M00_L00
M00_L01:
       cmp       r8d,edx
       jle       short M00_L03
M00_L02:
       movsxd    r10,edx
       mov       [rcx+r10*4],edx
       inc       edx
       cmp       r8d,edx
       jg        short M00_L02
M00_L03:
       vzeroupper
       ret
; Total bytes of code 92
```

