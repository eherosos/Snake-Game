# Snake-Game
กฎของเกม
- ตัวละครผู้เล่นจะเริ่มต้นมาเพียงตัวเดียว และจะเคลื่อนที่ไปทางขวาในตอนเริ่มต้น
- ตัวละคร ศัตรู, ฮีโร่ และสัตว์ จะเกิดมาในทุกๆ 5 วินาที
- ผู้เล่นสามารถควบคุมทิศทางเคลื่อนที่ได้ด้วยปุ่มลูกศร
- กด SPACE และ C สลับตำแหน่งตัวละครหัวแถวกับรองหัวแถว
- เมื่อตัวละครหัวแถวของผู้เล่นเดินทับตัวละครที่ไม่ใช่แถวของตนเอง
  - หากเป็นตัวละครฮีโร่จะรับเข้าต่อหางแถว
  - หากเป็นตัวละครสัตว์ตัวละครหัวแถวจะได้รับพลังชีวิต 1 หรือ 2
  - หากเป็นตัวละครมอนสเตอร์จะทำการต่อสู้
- ผู้เล่นจะได้คะแนนจากการชนะจาก ค่าพลังชีวิตที่มีของฮีโร่ที่ครองครอบอยู่รวมกัน
- ตัวละครไม่สามารถเคลื่อนที่ไปยังทิศตรงข้ามในขณะที่อยู่ในทิศปัจจุบัน
- หากคู่ต่อสู้เป็นประเภทเดียวกันพลังโจมตีจะคูณสอง
- ตัวละครไม่สามารถชนกำแพง หากชนแล้วจะต้องเสียตัวละครหัวแถว 1 ตัว แล้วจะจัดทิศทางใหม่ให้เอง แต่ถ้าไม่เหลือแล้วเกมจะจบลงทันที
- เป้าหมายของเกมคือ สะสมคะแนนให้ได้มากที่สุด

สิ่งที่เพิ่มเข้ามา
- ตัวเกมได้มีการเพิ่มตัวละครเพิ่มประเภทสัตว์เข้ามาในพื้นที่เพิ่มมาเป็นค่าพลังชีวิตให้กับตัวละคร เมื่อเกมเข้าสู่รอบที่ 10 เป็นต้นไป โดยที่จะโอกาสเกิดเพียง 25%
- การจัดประเภทของตัวละครได้จัดในไว้ Character Category.jpg