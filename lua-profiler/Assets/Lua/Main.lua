--主入口函数。从这里开始lua逻辑

function TestUpdate1 ()
    -- print ('Test Update1')
end

function TestUpdate2 ()
    -- print ('Test Update2')
end

function Update ()
    TestUpdate1 ()
    TestUpdate2 ()
end

function Main ()
	print("logic start")
    UpdateBeat:Add (Update, self)
end

function OnApplicationQuit ()

end


