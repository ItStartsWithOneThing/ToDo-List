
//Autoresizing text area while entering text
const resizeTextarea = (event) => {
    let currentEl = event.currentTarget;

    currentEl.style.height = 'auto'; // Narrow

    currentEl.style.height = currentEl.scrollHeight + 'px'; // Expand
}

//Translates priority value(1/2/3) into text
const getPriorityText = (priorityValue) => {
    switch (priorityValue) {
        case 1:
            return 'Low';
            break;
        case 2:
            return 'Medium';
            break;
        case 3:
            return 'High';
            break;
        default:
            return 'Low';
    }
} 

//Shows Background colors options
const showBGColors = (event) => {
    const cardFormBackground = event.currentTarget.parentNode.parentNode;

    const bgColorsContainer = document.querySelector(".bg-color-container");
    bgColorsContainer.classList.toggle("bg-colors-container-show");

    const colorOptions = document.querySelectorAll(".color-option");

    //Adding appropriate BG color to each color-option and enabling them to close color pannel by clicking them
    colorOptions.forEach((option) => {
        option.style.backgroundColor = option.getAttribute('data-color');
        option.addEventListener('click', function () {
            let selectedColor = option.getAttribute('data-color');
            cardFormBackground.style.backgroundColor = selectedColor;
        });
    });
}

//Shows when card was updated last time
const showEditedTime = (editedDate) => {
    let result = "Edited: ";

    let now = new Date();
    let then = new Date(editedDate);
    let yearsDiff = now.getFullYear() - then.getFullYear();
    let mothDiff = now.getMonth() - then.getMonth();
    let daysDiff = now.getDate() - then.getDate();

    if ((daysDiff + mothDiff + yearsDiff) == 0) {
        result += then.getHours() + ":";
        result += then.getMinutes() < 10 ? `0` + `${then.getMinutes()}` : `${then.getMinutes()}`; // if minutes less than 10, then add 0-prefix before minutes (:07min) 
    }

    if (yearsDiff == 0 && (daysDiff != 0 || mothDiff != 0)) {
        result += then.getDate() + " " + then.toLocaleString('en-EN', { month: 'short' });
    }

    if (yearsDiff > 0) {
        result += then.getDate() + " " + then.toLocaleString('en-EN', { month: 'short' }) + ". " + then.getFullYear();
    }

    return result;
}


const closeModalWindow = () => {
    const cardModal = document.querySelector(".card-modal");
    cardModal.classList.remove("modal-show");
    cardModal.innerHTML = "";
};

const getLocaldateTimeString = (date) => {
    function checkNum(number) {
        if (number > 9) {
            return `${number}`;
        }
        return `0${number}`;
    }
    let cDate = date.getFullYear() + '-' + checkNum(date.getMonth() + 1) + '-' + checkNum(date.getDate());
    let cTime = checkNum(date.getHours()) + ":" + checkNum(date.getMinutes()) + ":" + checkNum(date.getSeconds());
    let dateTime = cDate + 'T' + cTime;
    return dateTime;
}

export { resizeTextarea, getPriorityText, showBGColors, showEditedTime, closeModalWindow, getLocaldateTimeString };