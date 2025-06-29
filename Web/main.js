document.getElementById("fileInput").addEventListener("change", handleFiles);

function handleFiles(event) {
  const files = event.target.files;
  for (let file of files) {
    processFile(file);
  }
}

function processFile(file) {
  const reader = new FileReader();
  reader.onload = function (e) {
    const base64 = e.target.result;

    EXIF.getData(file, function () {
      const dateTime = EXIF.getTag(this, "DateTimeOriginal");
      const formatted = formatExifDate(dateTime);

      const payload = JSON.stringify({
        image: base64,
        date: formatted.date,
        day: formatted.day,
        time: formatted.time,
      });

      if (window.unityInstance && window.unityInstance.SendMessage) {
        window.unityInstance.SendMessage("GameManager", "OnReceiveImageData", payload);
      } else {
        console.warn("⚠ Unity 未読み込み、仮表示中:");
        console.log(payload);
      }
    });
  };
  reader.readAsDataURL(file);
}

function formatExifDate(str) {
  if (!str) return { date: "???", day: "?", time: "??:??:??" };

  const parts = str.split(/[: ]/); // ["2025", "06", "29", "14", "30", "00"]
  const dateObj = new Date(`${parts[0]}-${parts[1]}-${parts[2]}T${parts[3]}:${parts[4]}:${parts[5]}`);
  const weekdays = ["日", "月", "火", "水", "木", "金", "土"];
  const day = weekdays[dateObj.getDay()];

  return {
    date: `${parts[0]}/${parts[1]}/${parts[2]}`,
    day: day,
    time: `${parts[3]}:${parts[4]}:${parts[5]}`
  };
}
