local CAN_WHIP_TIMER = 0x0457
local T = 0
local prevT = -1
local canWhip = true
local inCountdown = false
local isWhipping = false

local startWhipFrame = -1
local endWhipFrame = -1
local whipDelta = -1
local prevInput = {}
local missedWhip = false

emu.registerbefore(function()
    T = memory.readbyteunsigned(CAN_WHIP_TIMER)

    local currentInput = joypad.read(1)


    local frameCount = emu.framecount()

    if T == 4 and prevT == 0 and isWhipping == false then
        isWhipping = true
        startWhipFrame = frameCount

        if endWhipFrame ~= -1 then
            whipDelta = startWhipFrame - endWhipFrame
        end
    end

    if T == 8 then 
        inCountdown = true
    elseif inCountdown == true and T == 0 then
        canWhip = true
        inCountdown = false
        isWhipping = false
        endWhipFrame = frameCount
    elseif inCountdown == false and T > 0 then
        canWhip = false
    end

    -- Check if B is pressed now, but was NOT pressed last frame
    if currentInput['B'] and not prevInput['B'] and isWhipping then
        missedWhip = true
    else
        missedWhip = false
    end

    prevT = T
end)

gui.register(function()
    if whipDelta ~= -1 then
        local missAmount = whipDelta - 2
        local color = "red"
        if missAmount == 0 then
            color = "green"
        end
        gui.text(185, 20, string.format("Miss: %02d", missAmount), color, "black")
    end
    if canWhip then
        gui.box(185, 28, 205, 45, "green", "white")
    end
    if isWhipping then
        gui.box(210, 28, 230, 45, "yellow", "black")
    end
    -- if missedWhip then
    --     gui.box(235, 28, 255, 45, "red", "black")
    -- end
end)

-- keep script alive
while true do 
    emu.frameadvance() 
end
