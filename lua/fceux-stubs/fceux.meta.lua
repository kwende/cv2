---@meta
---@diagnostic disable: missing-return, unused-local

-- ============================================================
-- FCEUX Lua API stubs (for LuaLS / VS Code IntelliSense)
-- Based on the Lua Functions List you pasted.
-- ============================================================

-- ----------------------------
-- Common helper types
-- ----------------------------

---@alias CPURegisterName '"a"'|'"x"'|'"y"'|'"s"'|'"p"'|'"pc"'
---@alias SpeedMode '"normal"'|'"nothrottle"'|'"turbo"'|'"maximum"'

---@alias Color
---| integer
---| string
---| { [1]:integer, [2]:integer, [3]:integer, [4]?:integer }         -- {r,g,b,a}
---| { r:integer, g:integer, b:integer, a?:integer }                -- {r=,g=,b=,a=}

---@alias JoypadButton '"up"'|'"down"'|'"left"'|'"right"'|'"A"'|'"B"'|'"start"'|'"select"'
---@alias JoypadValue boolean|string|nil  -- true/false/nil/"invert" (any string counts as invert)

---@class JoypadInput
---@field up? JoypadValue
---@field down? JoypadValue
---@field left? JoypadValue
---@field right? JoypadValue
---@field A? JoypadValue
---@field B? JoypadValue
---@field start? JoypadValue
---@field select? JoypadValue

---@alias MemoryHook fun(address:integer, size:integer, value:integer)

-- ============================================================
-- emu library
-- ============================================================
---@class emu
emu = {}

function emu.poweron() end
function emu.softreset() end

---@param mode SpeedMode
function emu.speedmode(mode) end

function emu.frameadvance() end
function emu.pause() end
function emu.unpause() end

---@param count integer
---@param func fun()
function emu.exec_count(count, func) end

---@param time integer
---@param func fun()
function emu.exec_time(time, func) end

---@param sprites boolean|nil
---@param background boolean|nil
function emu.setrenderplanes(sprites, background) end

---@param message string
function emu.message(message) end

---@return integer
function emu.framecount() end

---@return integer
function emu.lagcount() end

---@return boolean
function emu.lagged() end

---@param value boolean
function emu.setlagflag(value) end

---@return boolean
function emu.emulating() end

---@return boolean
function emu.paused() end

---@return boolean
function emu.readonly() end

---@param state boolean
function emu.setreadonly(state) end

---@return string
function emu.getdir() end

---@param filename string
function emu.loadrom(filename) end

---@param func fun()|nil
---@return fun()|nil old
function emu.registerbefore(func) end

---@param func fun()|nil
---@return fun()|nil old
function emu.registerafter(func) end

---@param func fun()|nil
---@return fun()|nil old
function emu.registerexit(func) end

---@param str string
---@return boolean ok
---@return string? errorMessage
function emu.addgamegenie(str) end

---@param str string
---@return boolean ok
---@return string? errorMessage
function emu.delgamegenie(str) end

---@param str string
function emu.print(str) end

---@param x integer
---@param y integer
---@param getemuscreen boolean
---@return integer r
---@return integer g
---@return integer b
---@return integer palette -- 0-63 or 254 on error
function emu.getscreenpixel(x, y, getemuscreen) end

function emu.exit() end

-- Backwards-compat alias
---@class FCEU : emu
FCEU = emu

-- ============================================================
-- rom library
-- ============================================================
---@class rom
rom = {}

---@return string
function rom.getfilename() end

---@param type '"md5"'|'"base64"'
---@return string
function rom.gethash(type) end

---@param address integer
---@return integer
function rom.readbyte(address) end

---@param address integer
---@return integer
function rom.readbyteunsigned(address) end

---@param address integer
---@return integer
function rom.readbytesigned(address) end

---@param address integer
---@param value integer
function rom.writebyte(address, value) end

-- ============================================================
-- memory library
-- ============================================================
---@class memory
memory = {}

---@param address integer
---@return integer
function memory.readbyte(address) end

---@param address integer
---@return integer
function memory.readbyteunsigned(address) end

---@param address integer
---@param length integer
---@return string
function memory.readbyterange(address, length) end

---@param address integer
---@return integer
function memory.readbytesigned(address) end

---@param addressLow integer
---@param addressHigh? integer
---@return integer
function memory.readword(addressLow, addressHigh) end

---@param addressLow integer
---@param addressHigh? integer
---@return integer
function memory.readwordunsigned(addressLow, addressHigh) end

---@param addressLow integer
---@param addressHigh? integer
---@return integer
function memory.readwordsigned(addressLow, addressHigh) end

---@param address integer
---@param value integer
function memory.writebyte(address, value) end

---@param cpuregistername CPURegisterName
---@return integer
function memory.getregister(cpuregistername) end

---@param cpuregistername CPURegisterName
---@param value integer
function memory.setregister(cpuregistername, value) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.register(address, size, func) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.registerread(address, size, func) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.registerwrite(address, size, func) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.registerexec(address, size, func) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.registerrun(address, size, func) end

---@overload fun(address:integer, func:MemoryHook|nil):nil
---@param address integer
---@param size integer
---@param func MemoryHook|nil
function memory.registerexecute(address, size, func) end

-- ============================================================
-- ppu library
-- ============================================================
---@class ppu
ppu = {}

---@param address integer
---@return integer
function ppu.readbyte(address) end

---@param address integer
---@param length integer
---@return string
function ppu.readbyterange(address, length) end

-- ============================================================
-- debugger library
-- ============================================================
---@class debugger
debugger = {}

function debugger.hitbreakpoint() end

---@return integer
function debugger.getcyclescount() end

---@return integer
function debugger.getinstructionscount() end

function debugger.resetcyclescount() end
function debugger.resetinstructionscount() end

---@param name string
---@param bank? integer
---@return integer offset -- -1 if not found
function debugger.getsymboloffset(name, bank) end

-- ============================================================
-- joypad library
-- ============================================================
---@class joypad
joypad = {}

---@param player integer
---@return table<JoypadButton, boolean>
function joypad.get(player) end

---@param player integer
---@return table<JoypadButton, boolean>
function joypad.read(player) end

---@param player integer
---@return table<JoypadButton, boolean>|nil
function joypad.getimmediate(player) end

---@param player integer
---@return table<JoypadButton, boolean>|nil
function joypad.readimmediate(player) end

---@param player integer
---@return table<JoypadButton, true>
function joypad.getdown(player) end

---@param player integer
---@return table<JoypadButton, true>
function joypad.readdown(player) end

---@param player integer
---@return table<JoypadButton, false>
function joypad.getup(player) end

---@param player integer
---@return table<JoypadButton, false>
function joypad.readup(player) end

---@param player integer
---@param input JoypadInput
function joypad.set(player, input) end

---@param player integer
---@param input JoypadInput
function joypad.write(player, input) end

-- ============================================================
-- zapper library
-- ============================================================
---@class ZapperState
---@field x integer
---@field y integer
---@field fire integer -- 0/1

---@class zapper
zapper = {}

---@return ZapperState
function zapper.read() end

---@param input {x?:integer|nil, y?:integer|nil, fire?:boolean|integer|nil}
function zapper.set(input) end

-- ============================================================
-- input library
-- ============================================================
---@class InputState
---@field xmouse integer
---@field ymouse integer
---@field [string] boolean|nil

---@class input
input = {}

---@return InputState
function input.get() end

---@return InputState
function input.read() end

---@param message string
---@param type? '"ok"'|'"yesno"'|'"yesnocancel"'|'"okcancel"'|'"abortretryignore"'
---@param icon? '"message"'|'"question"'|'"warning"'|'"error"'
---@return string button
function input.popup(message, type, icon) end

-- ============================================================
-- savestate library
-- ============================================================
---@class SavestateObject
local SavestateObject = {}

---@class savestate
savestate = {}

---@param slot? integer
---@return SavestateObject
function savestate.object(slot) end

---@param slot? integer
---@return SavestateObject
function savestate.create(slot) end

---@param state SavestateObject
function savestate.save(state) end

---@param state SavestateObject
function savestate.load(state) end

---@param state SavestateObject
function savestate.persist(state) end

---@param func fun()|nil
---@return fun()? old
function savestate.registersave(func) end

---@param func fun()|nil
---@return fun()? old
function savestate.registerload(func) end

---@param location integer
---@return any
function savestate.loadscriptdata(location) end

-- ============================================================
-- movie library
-- ============================================================
---@class movie
movie = {}

---@param filename string
---@param read_only? boolean
---@param pauseframe? integer
---@return boolean ok
function movie.play(filename, read_only, pauseframe) end

---@return boolean ok
function movie.playback(...) end

---@return boolean ok
function movie.load(...) end

---@param filename string
---@param save_type? integer
---@param author? string
---@return boolean ok
function movie.record(filename, save_type, author) end

---@return boolean ok
function movie.save(...) end

---@return boolean
function movie.active() end

---@return integer
function movie.framecount() end

---@return '"record"'|'"playback"'|'"finished"'|'"taseditor"'|nil
function movie.mode() end

---@param counting boolean
function movie.rerecordcounting(counting) end

function movie.stop() end
function movie.close() end

---@return integer
function movie.length() end

---@return string
function movie.name() end

---@return string
function movie.getname() end

---@return string
function movie.getfilename() end

---@return integer
function movie.rerecordcount() end

function movie.replay() end
function movie.playbeginning() end

---@return boolean
function movie.readonly() end

---@return boolean
function movie.getreadonly() end

---@param state boolean
function movie.setreadonly(state) end

---@return boolean
function movie.recording() end

---@return boolean
function movie.playing() end

---@return boolean
function movie.ispoweron() end

---@return boolean
function movie.isfromsavestate() end

-- ============================================================
-- gui library
-- ============================================================
---@class gui
gui = {}

---@param x integer
---@param y integer
---@param color Color
function gui.pixel(x, y, color) end
gui.drawpixel = gui.pixel
gui.setpixel  = gui.pixel
gui.writepixel = gui.pixel

---@param x integer
---@param y integer
---@return integer r
---@return integer g
---@return integer b
---@return integer a
function gui.getpixel(x, y) end

---@param x1 integer
---@param y1 integer
---@param x2 integer
---@param y2 integer
---@param color? Color
---@param skipfirst? boolean
function gui.line(x1, y1, x2, y2, color, skipfirst) end
gui.drawline = gui.line

---@param x1 integer
---@param y1 integer
---@param x2 integer
---@param y2 integer
---@param fillcolor? Color
---@param outlinecolor? Color
function gui.box(x1, y1, x2, y2, fillcolor, outlinecolor) end
gui.drawbox = gui.box
gui.rect = gui.box
gui.drawrect = gui.box

---@param x integer
---@param y integer
---@param str string
---@param textcolor? Color
---@param backcolor? Color
function gui.text(x, y, str, textcolor, backcolor) end
gui.drawtext = gui.text

---@param color Color
---@return integer r
---@return integer g
---@return integer b
---@return integer a
function gui.parsecolor(color) end

function gui.savescreenshot() end
---@param name string
function gui.savescreenshotas(name) end

---@param getemuscreen boolean
---@return string gdString
function gui.gdscreenshot(getemuscreen) end

---@param dx? integer
---@param dy? integer
---@param str string
---@param sx? integer
---@param sy? integer
---@param sw? integer
---@param sh? integer
---@param alphamul? number
function gui.gdoverlay(dx, dy, str, sx, sy, sw, sh, alphamul) end
gui.image = gui.gdoverlay
gui.drawimage = gui.gdoverlay

---@param alpha number
function gui.opacity(alpha) end

---@param trans number
function gui.transparency(trans) end

---@param func fun()|nil
function gui.register(func) end

---@param message string
---@param type? '"ok"'|'"yesno"'|'"yesnocancel"'|'"okcancel"'|'"abortretryignore"'
---@param icon? '"message"'|'"question"'|'"warning"'|'"error"'
---@return string button
function gui.popup(message, type, icon) end

-- ============================================================
-- sound library
-- ============================================================
---@class SoundChannelRegs
---@field frequency number

---@class SoundChannel
---@field volume number
---@field frequency number
---@field midikey integer
---@field duty? integer
---@field short? boolean
---@field dmcaddress? integer
---@field dmcsize? integer
---@field dmcloop? boolean
---@field dmcseed? integer
---@field regs SoundChannelRegs

---@class RP2A03State
---@field square1 SoundChannel
---@field square2 SoundChannel
---@field triangle SoundChannel
---@field noise SoundChannel
---@field dpcm SoundChannel

---@class SoundState
---@field rp2a03 RP2A03State

---@class sound
sound = {}

---@return SoundState
function sound.get() end

-- ============================================================
-- taseditor library (signatures minimal; docs elsewhere)
-- ============================================================
---@class taseditor
taseditor = {}

---@param func fun()|nil
function taseditor.registerauto(func) end

---@param func fun()|nil
function taseditor.registermanual(func) end

---@return boolean
function taseditor.engaged() end

---@param frame integer
---@return boolean
function taseditor.markedframe(frame) end

---@param frame integer
---@return integer
function taseditor.getmarker(frame) end

---@param frame integer
---@return integer
function taseditor.setmarker(frame) end

---@param frame integer
function taseditor.clearmarker(frame) end

---@param index integer
---@return string
function taseditor.getnote(index) end

---@param index integer
---@param newtext string
function taseditor.setnote(index, newtext) end

---@return integer
function taseditor.getcurrentbranch() end

---@return string
function taseditor.getrecordermode() end

---@return integer
function taseditor.getsuperimpose() end

---@return integer
function taseditor.getlostplayback() end

---@return integer
function taseditor.getplaybacktarget() end

---@param frame integer
function taseditor.setplayback(frame) end

function taseditor.stopseeking() end

function taseditor.getselection() end
function taseditor.setselection() end

---@param frame integer
---@param joypad integer
---@return integer
function taseditor.getinput(frame, joypad) end

---@param frame integer
---@param joypad integer
---@param inputVal integer
function taseditor.submitinputchange(frame, joypad, inputVal) end

---@param frame integer
---@param number integer
function taseditor.submitinsertframes(frame, number) end

---@param frame integer
---@param number integer
function taseditor.submitdeleteframes(frame, number) end

---@param name? string
---@return integer
function taseditor.applyinputchanges(name) end

function taseditor.clearinputchanges() end

-- ============================================================
-- bitwise helpers
-- ============================================================

---@vararg integer
---@return integer
function AND(...) end

---@vararg integer
---@return integer
function OR(...) end

---@vararg integer
---@return integer
function XOR(...) end

---@vararg integer
---@return integer
function BIT(...) end
