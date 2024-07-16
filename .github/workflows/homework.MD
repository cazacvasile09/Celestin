# Homework - Deadline 2 days

## Intro 

After this short presentation, you should be able to create a small pipeline that is able to build a Docker image.

How to proceed: seems that you already have your own branches, so add the Dockerfile and .github/workflows/example.yaml **or** merge the **cicd** branch into yours.

Save your progress first: 
```
Mandatory: git checkout <your-branch>

git checkout -b <your-branch>-save

git pull

git checkout <your-branch>

git merge cicd

```

### CI/CD

Build a pipeline that does the following tasks:
* uses the application code
* builds a Docker image with your app
* login into docker hub
* pushes the image to the DockerHub
* **EXTRAPOINTS - try to not use the credentials in plain text**
* **Even more extrapoints - if you wrote some type of test, you can test the application as well in the pipeline; *any attempt will be taken in consideration***

### Docker - optional; additional points

First of all, dive deeper into Docker architecture and how it works behind the scenes. It's pretty critical.

###### Task:
I gave you the Dockerfile for you application, but that Dockerfile can be improved. 
> Hint: Layers

Modify the Dockerfile accordingly and add a new step in the pipeline with the difference and what did you observe.

> You can test your application locally, if you install docker and docker-compose. You'll have to dockerize the database as well.

Credentials:

* user: practicaendvsv
* password: iNorzWtcMEuS88iDPlOgkw


> have a look on google: how to hide credentials in github actions

### WSL + docker installation

1. https://learn.microsoft.com/en-us/windows/wsl/install

2. https://devblogs.microsoft.com/commandline/systemd-support-is-now-available-in-wsl/

3. https://docs.docker.com/engine/install/

> you can have a look on docker-compose as well