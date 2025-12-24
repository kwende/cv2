local CAN_WHIP_TIMER = 0x0457
local T = 0
local canWhip = true
local inCountdown = false

emu.registerbefore(function()
    T = memory.readbyteunsigned(CAN_WHIP_TIMER)

    if T == 8 then 
        inCountdown = true
    elseif inCountdown == true and T == 0 then
        canWhip = true
        inCountdown = false
    elseif inCountdown == false and T > 0 then
        canWhip = false
    end
end)

gui.register(function()
    gui.text(185, 8, string.format("WhipCD: %02d", T), "white", "black")
    if canWhip then
        gui.box(230, 20, 255, 45, "green", "white")
        -- gui.text(200, 28, string.format("WhipCD: %02d", lastT), "yellow", "black")
    end
end)

-- keep script alive
while true do emu.frameadvance() end
