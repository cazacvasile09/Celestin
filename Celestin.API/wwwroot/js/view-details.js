const BASE_URL = 'https://localhost:5001/api';
const LIST_ITEMS = '/celestin';

const NAV_TITLE_LIST = "List";
const NAV_TITLE_ADD = "Add new item";
const NAV_TITLE_EDIT = "Edit item";
const NAV_TITLE_ABOUT = "About";

const NAV_PATH_BASE = "./";

const NAV_PATH_LIST = "Index";
const NAV_PATH_ADD = "ViewDetails";
const NAV_PATH_ABOUT = "About";

const NAV_ID_BASE = "link-item";

const NAV_ID_LIST = "list";
const NAV_ID_ADD = "add";
const NAV_ID_ABOUT = "about";


const links = [
    {
        path: `${NAV_PATH_BASE}${NAV_PATH_LIST}`,
        title: NAV_TITLE_LIST,
        id: NAV_ID_LIST,
    },
    {
        path: `${NAV_PATH_BASE}${NAV_PATH_ADD}`,
        title: NAV_TITLE_ADD,
        id: NAV_ID_ADD,
    },
    {
        path: `${NAV_PATH_BASE}${NAV_PATH_ABOUT}`,
        title: NAV_TITLE_ABOUT,
        id: NAV_ID_ABOUT,
    },
];


function setInputValue(id, value, defaultValue = "-") {
    const inputElement = document.getElementById(id);
    inputElement.value = value || defaultValue;
}

function updateNavbarContent(id, value) {
    const linkEl = document.getElementById(id);
    linkEl.textContent = value;
}

function getNewElement(type, attributes) {
    const element = document.createElement(type);
    if (attributes) {
        attributes.forEach((attribute) => {
            element.setAttribute(attribute.name, attribute.value);
        });
    }
    return element;
}

function getNewLink(text, href, id) {
    const linkEl = getNewElement("a", [{ name: "id", value: id }]);
    var linkNode = document.createTextNode(text);
    linkEl.appendChild(linkNode);
    linkEl.title = text;
    linkEl.href = href;

    return linkEl;
}

function getNavbar(activePage) {
    const navElement = getNewElement("nav", [{ name: "class", value: "navbar" }]);

    const ulElement = getNewElement("ul", []);
    links.forEach((link) => {
        const liElement = getNewElement("li");
        const aElement = getNewLink(
            link.title,
            link.path,
            `${NAV_ID_BASE}-${link.id}`,
        );
        if (link.title === activePage) {
            aElement.setAttribute("class", "active");
        }
        liElement.appendChild(aElement);
        ulElement.appendChild(liElement);
    });
    navElement.appendChild(ulElement);
    return navElement;
}

function populateForm(element) {
    setInputValue("input-name", element.name);
    setInputValue("input-mass", element.mass || "");
    setInputValue("input-diameter", element.equatorialDiameter || 0);
    setInputValue("input-temperature", element.surfaceTemperature || "");
    setInputValue("input-date", element.discoveryDate || "");
}

function hideElement(id) {
    const element = document.getElementById(id);
    element.setAttribute("class", "hidden");
}

function showElement(id) {
    const element = document.getElementById(id);
    element.removeAttribute("class", "hidden");
}

function setTexts(titleContent, submitContent) {
    document.getElementById("heading-title").innerHTML = titleContent;
    document.getElementById("input-submit").value = submitContent;
}

function updateContent(isEditMode) {
    if (isEditMode) {
        setTexts("Edit element", "Save");
        updateNavbarContent(`${NAV_ID_BASE}-${NAV_ID_ADD}`, NAV_TITLE_EDIT);
    } else {
        setTexts("Add new element", "Add");
        hideElement("loader-container");
    }
}

async function fetchData(url) {
    try {
        const response = await fetch(url, {
            mode: 'cors', // Changed from 'no-cors' to 'cors'
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            },
        });
        if (!response.ok) {
            alert('Data not available..');
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const result = await response.json();
        return result;
    } catch (error) {
        console.error('Failed to fetch:', error);
        throw error;
    }
}

async function getData(id) {
    hideElement("form");

    const url = `${BASE_URL}${LIST_ITEMS}/${id}`;
    try {
        const data = await fetchData(url);
        populateForm(data);
        hideElement("loader-container");
        showElement("form");
    } catch (error) {
        console.error('Error fetching data:', error);
        alert('Failed to load data.');
    }
}

function getIdParam() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get("id");
}

window.addEventListener("load", async () => {
    document.getElementById("navbar").appendChild(getNavbar(NAV_TITLE_ADD));
    const objId = getIdParam();

    updateContent(objId !== null);

    if (objId !== null) {
        await getData(objId);
    }
});

function getObjectFromElements() {
    return {
        Name: document.getElementById("input-name").value,
        Mass: parseFloat(document.getElementById("input-mass").value),
        EquatorialDiameter: parseInt(document.getElementById("input-diameter").value),
        SurfaceTemperature: parseInt(document.getElementById("input-temperature").value),
        DiscoveryDate: document.getElementById("input-date").value,
        DiscoverySourceId: 1
    };
}

async function add(data) {
    const response = await fetch(`${BASE_URL}${LIST_ITEMS}/CreateCelestin`, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "same-origin", // include, *same-origin, omit
        headers: {
            "Content-Type": "application/json",
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data), // body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
}

document.addEventListener("submit", async function (e) {
    e.preventDefault();
    const objId = getIdParam();
    const myObj = getObjectFromElements();

    if (objId) {
        // Handle update
        const updateUrl = `${BASE_URL}${LIST_ITEMS}/${objId}`;
        try {
            const response = await fetch(updateUrl, {
                method: "PATCH",  // Assuming PATCH method is used for updates
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(myObj),
            });

            if (!response.ok) {
                throw new Error('Failed to update item');
            }

            // Optionally handle response or redirect after successful update
            alert('Item updated successfully!');
            // Redirect or update UI as needed

        } catch (error) {
            console.error('Error updating item:', error);
            alert('Failed to update item');
        }

    } else {
        // Handle add (if necessary)
        alert('No item ID found for update.');
    }
});
