const BASE_URL = 'https://localhost:44335/api';
const LIST_ITEMS = '/celestin';

const NAV_TITLE_LIST = "List";
const NAV_TITLE_ADD = "Add new item";
const NAV_TITLE_EDIT = "Edit item";
const NAV_TITLE_ABOUT = "About";

const NAV_PATH_BASE = "./";

const NAV_PATH_LIST = "Index";
const NAV_PATH_ADD = "viewdetails";
const NAV_PATH_ABOUT = "about";

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


async function fetchData(url) {
    try {
        const response = await fetch(url, {
            mode: 'no-cors',
            method: "GET",
            headers: {
                "Access-Control-Allow-Headers": "Content-Type",
                "Access-Control-Allow-Origin": "*",
                'Content-Type': 'application/json',
                "Access-Control-Allow-Methods": "OPTIONS,POST,GET,PATCH"
            },
        });
        if (!response.ok) {
            alert('Error');
        }
        const result = await response.json();
        return result;
    } catch (error) {

        console.error(error);
        throw error;
    }
}

async function fetchFilteredData() {
    const searchSelect = document.getElementById('searchSelect');
    const searchInput = document.getElementById('searchInput');

    const searchFieldValue = searchSelect.value;
    const searchValue = searchInput.value;

    const url = `${BASE_URL}${LIST_ITEMS}${searchFieldValue}=${searchValue}`;
    const data = await fetchData(url);
    const divTable = document.getElementById('table');
    if (divTable.firstChild) {
        divTable.firstChild.remove();
    }

    const table = generateTable(data);
    divTable.appendChild(table);
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

function generateTable(items) {
    const table = getNewElement('table');
    const headerRow = getNewElement('tr');
    const headersCollumnsName = ['Name', 'Mass', 'Equatorial Diameter', 'Surface Temperture', 'Discovery Date', 'Actions'];

    headersCollumnsName.forEach(element => {
        const tableHeaderCell = getNewElement('th');
        const tableHeaderParagraph = getNewParagraph(element);
        tableHeaderCell.appendChild(tableHeaderParagraph);
        headerRow.appendChild(tableHeaderCell);
    });
    table.appendChild(headerRow);

    const itemAttributes = ['name', 'mass', 'equatorialDiameter', 'surfaceTemperature', 'discoveryDate'];

    items.forEach(element => {
        const tableDataRow = getNewElement('tr');
        itemAttributes.forEach(attributeValue => {

            const tableDataCell = getNewElement('td');
            const tableDataParagraph = getNewParagraph(element[attributeValue]);

            tableDataCell.appendChild(tableDataParagraph);
            tableDataRow.appendChild(tableDataCell);
        });

        const tableDataActionCell = getNewElement('td');
        const linkElement = getNewLink(
            "View more",
            `ViewDetails?id=${element.id}`,
        );
        tableDataActionCell.appendChild(linkElement);
        tableDataRow.appendChild(tableDataActionCell);

        table.appendChild(tableDataRow);
    });

    return table;
}

function getNewParagraph(content, attributes) {
    const paragraphEl = getNewElement("p", attributes);
    const node = document.createTextNode(content);
    paragraphEl.appendChild(node);
    return paragraphEl;
}

function getNewLink(text, href) {
    const linkEl = getNewElement("a");
    var linkNode = document.createTextNode(text);
    linkEl.appendChild(linkNode);
    linkEl.title = text;
    linkEl.href = href;

    return linkEl;
}

function getNavbar(activePage) {

    const navElement = getNewElement("nav", [{ name: "class", value: "navbar" }]);

    const ulElement = getNewElement("ul", [{ name: "class", value: "test" }]);
    links.forEach((link) => {
        const liElement = getNewElement("li");
        const aElement = getNewLink(link.title, link.path);
        if (link.title === activePage) {
            aElement.setAttribute("class", "active");
        }
        liElement.appendChild(aElement);
        ulElement.appendChild(liElement);
    });
    navElement.appendChild(ulElement);
    return navElement;
}


function renderItem(element) {
    const ulElement = document.getElementById("list");
    const li = getNewElement("li", [{ name: "class", value: "item" }]);
    const divElement = getNewElement("div");

    const paragraphElementName = getNewParagraph(element.name, [
        { name: "id", value: "p-name" },
        { name: "class", value: "pName" },
    ]);
    const divElementData = getNewElement("div", [
        { name: "class", value: "data-container" },
    ]);

    if (element.data && element.data.color) {
        const colorParagraph = getNewParagraph(`Color: ${element.data.color}`);
        divElementData.appendChild(colorParagraph);
    }

    const linkElement = getNewLink(
        "View more",
        `view-details.html?id=${element.Id}`,
    );

    divElement.appendChild(paragraphElementName);
    divElement.appendChild(divElementData);
    divElement.appendChild(linkElement);

    li.appendChild(divElement);
    ulElement.appendChild(li);
}


function addNavbar() {
    const navBarContent = getNavbar(NAV_TITLE_LIST);
    document.getElementById("navbar").appendChild(navBarContent);
}

async function readData() {

    document.getElementById("dynamic-content-container").setAttribute("class", "hidden");

    const url = `${BASE_URL}${LIST_ITEMS}`;
    const data = await fetchData(url);

    document.getElementById("loader-container").setAttribute("class", "hidden");
    document.getElementById("dynamic-content-container").removeAttribute("class", "hidden");

    const divTable = document.getElementById('table');
    const table = generateTable(data);

    divTable.appendChild(table);
}

window.addEventListener("load", () => {
    addNavbar();

    readData();
});
