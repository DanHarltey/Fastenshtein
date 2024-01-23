## .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```assembly
; Fastenshtein.Benchmarking.ArrayBenchmark.Vector512Populate()
;             var tmp = _array;
;             ^^^^^^^^^^^^^^^^^
;             var i = 0;
;             ^^^^^^^^^^
;                 tmp[i] = i;
;                 ^^^^^^^^^^^
;             for (; i < tmp.Length; i++)
;                                    ^^^
;             return tmp;
;             ^^^^^^^^^^^
       mov       rax,[rcx+8]
       xor       ecx,ecx
       mov       edx,[rax+8]
       test      edx,edx
       jle       short M00_L01
M00_L00:
       mov       r8d,ecx
       mov       [rax+r8*4+10],ecx
       inc       ecx
       cmp       edx,ecx
       jg        short M00_L00
M00_L01:
       ret
; Total bytes of code 28
```

