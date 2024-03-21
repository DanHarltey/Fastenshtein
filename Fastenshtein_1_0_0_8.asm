; Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8.DistanceFrom(System.String)
       push      r15
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rax,[rcx+10]
       cmp       dword ptr [rax+8],0 -- costs.Length == 0
       je        near ptr RETURN_VALUE_LENGTH
       xor       r8d,r8d
       cmp       dword ptr [rax+8],0
       jle       short OUTTER_LOOP_INTRO
ARRAY_POPULATE:
       lea       r10d,[r8+1]
       mov       r9,rax
       mov       r11d,[r9+8]
       cmp       r8d,r11d
       jae       near ptr OUT_OF_RANGE
       mov       r11d,r8d
       mov       [r9+r11*4+10],r10d
       cmp       [rax+8],r10d
       mov       r8d,r10d
       jg        short ARRAY_POPULATE
OUTTER_LOOP_INTRO:
       xor       r8d,r8d
       mov       r10d,[rdx+8]
       test      r10d,r10d
       jle       near ptr RETURN
       mov       rcx,[rcx+8]
OUTTER_LOOP:
       mov       r9d,r8d
       mov       r11d,r9d
       mov       ebx,r9d
       movzx     ebx,word ptr [rdx+rbx*2+0C]
       xor       esi,esi
       cmp       dword ptr [rcx+8],0
       jle       short OUTTER_LOOP_END
INNER_LOOP:
       mov       rdi,rax
       mov       ebp,[rdi+8]
       cmp       esi,ebp
       jae       near ptr OUT_OF_RANGE
       mov       r14d,esi
       mov       edi,[rdi+r14*4+10]
       mov       r15,rcx
       cmp       esi,[r15+8]
       jae       short OUT_OF_RANGE
       movzx     r15d,word ptr [r15+r14*2+0C]
       cmp       r15d,ebx
       je        short M01_L06
       cmp       r11d,r9d
       jge       short M01_L04
       mov       r9d,r11d
M01_L04:
       cmp       edi,r9d
       jge       short M01_L05
       mov       r9d,edi
M01_L05:
       inc       r9d
M01_L06:
       mov       r11,rax
       cmp       esi,ebp
       jae       short OUT_OF_RANGE
       mov       [r11+r14*4+10],r9d
       mov       r11d,r9d
       inc       esi
       cmp       [rcx+8],esi
       mov       r9d,edi
       jg        short INNER_LOOP
OUTTER_LOOP_END:
       inc       r8d
       cmp       r10d,r8d
       jg        short OUTTER_LOOP
RETURN:
       mov       rdx,rax
       mov       eax,[rax+8]
       dec       eax
       mov       ebp,[rdx+8]
       cmp       eax,ebp
       jae       short OUT_OF_RANGE
       mov       eax,[rdx+rax*4+10]
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       pop       r15
       ret
RETURN_VALUE_LENGTH:
       mov       r10d,[rdx+8]
       mov       eax,r10d
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       pop       r15
       ret
OUT_OF_RANGE:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 266