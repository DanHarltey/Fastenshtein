; Fastenshtein.Levenshtein.DistanceFrom(System.String)
       push      r15
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rax,[rcx+10]
       mov       r8d,[rax+8]
       test      r8d,r8d -- costs.Length == 0
       je        near ptr RETURN_VALUE_LENGTH
       xor       r10d,r10d
       test      r8d,r8d
       jle       short OUTTER_LOOP_INTRO
ARRAY_POPULATE:
       lea       r9d,[r10+1]
       mov       r10d,r10d
       mov       [rax+r10*4+10],r9d
       cmp       r8d,r9d -- r8d is cached cost length
       mov       r10d,r9d
       jg        short ARRAY_POPULATE
OUTTER_LOOP_INTRO:
       xor       r10d,r10d
       mov       r9d,[rdx+8]
       test      r9d,r9d
       jle       short RETURN
       mov       rcx,[rcx+8]
OUTTER_LOOP:
       mov       r11d,r10d
       mov       ebx,r11d
       mov       esi,r11d
       movzx     esi,word ptr [rdx+rsi*2+0C]
       xor       edi,edi
       cmp       dword ptr [rcx+8],0
       jle       short OUTTER_LOOP_END
       nop
INNER_LOOP:
       cmp       edi,r8d
       jae       short OUT_OF_RANGE
       mov       ebp,edi
       mov       r14d,[rax+rbp*4+10]
       mov       r15,rcx
       cmp       edi,[r15+8]
       jae       short OUT_OF_RANGE
       movzx     r15d,word ptr [r15+rbp*2+0C]
       cmp       r15d,esi
       je        short M01_L06
       cmp       ebx,r11d
       jge       short M01_L04
       mov       r11d,ebx
M01_L04:
       cmp       r14d,r11d
       jge       short M01_L05
       mov       r11d,r14d
M01_L05:
       inc       r11d
M01_L06:
       mov       [rax+rbp*4+10],r11d
       mov       ebx,r11d
       inc       edi
       cmp       [rcx+8],edi
       mov       r11d,r14d
       jg        short INNER_LOOP
OUTTER_LOOP_END:
       inc       r10d
       cmp       r9d,r10d
       jg        short OUTTER_LOOP
RETURN:
       dec       r8d
       mov       edx,r8d
       mov       eax,[rax+rdx*4+10]
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       pop       r15
       ret
RETURN_VALUE_LENGTH:
       mov       r9d,[rdx+8]
       mov       eax,r9d
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
; Total bytes of code 222